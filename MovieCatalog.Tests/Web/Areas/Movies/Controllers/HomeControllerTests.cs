namespace MovieCatalog.Tests.Web.Areas.Movies.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Moq;
    using MovieCatalog.Common;
    using MovieCatalog.Services.Data.Contracts;
    using MovieCatalog.Services.Data.Models.Movies;
    using MovieCatalog.Tests.Mocks;
    using MovieCatalog.Web.Areas.Movies.Controllers;
    using MovieCatalog.Web.Areas.Movies.Models;
    using Xunit;

    public class HomeControllerTests
    {
        private const int FirstMovieId = 1;
        private const int FirstMovieProductionYear = 2000;
        private const string FirstMovieTitle = "FirstMovieTitle";
        private const int SecondMovieId = 2;
        private const int SecondMovieProductionYear = 2001;
        private const string SecondMovieTitle = "SecondMovieTitle";
        private const int ThirdMovieId = 3;
        private const int ThirdMovieProductionYear = 2003;
        private const string ThirdMovieTitle = "ThirdMovieTitle";

        private const int InvalidMovieId = 100;

        #region Attributes

        #region HomeController_ShouldBeInMoviesArea

        [Fact]
        public void HomeController_ShouldBeInMoviesArea()
        {
            // Arrange
            Type controller = typeof(HomeController);

            // Act
            AreaAttribute areaAttribute = controller
                .GetCustomAttributes(true)
                .FirstOrDefault(a => a.GetType() == typeof(AreaAttribute))
                as AreaAttribute;

            // Assert
            areaAttribute.Should().NotBeNull();
            areaAttribute.RouteValue.Should().Be(WebConstants.MoviesArea);
        }

        #endregion HomeController_ShouldBeInMoviesArea

        #region Delete_ShouldOnlyBeForAdminUsers

        [Fact]
        public void Delete_ShouldOnlyBeForAdminUsers()
        {
            // Arrange
            MethodInfo method = typeof(HomeController)
                .GetMethod(nameof(HomeController.Delete));

            // Act
            AuthorizeAttribute authorizeAttribute = method
                .GetCustomAttributes(true)
                .FirstOrDefault(a => a.GetType() == typeof(AuthorizeAttribute))
                as AuthorizeAttribute;

            // Assert
            authorizeAttribute.Should().NotBeNull();
            authorizeAttribute.Roles.Should().Be(GlobalConstants.AdministratorRole);
        }

        #endregion Delete_ShouldOnlyBeForAdminUsers

        #endregion Attributes

        #region Index

        #region Index_ShouldReturnNotFoundWhenAjaxRequestAndInvalidMovieId

        [Fact]
        public async Task Index_ShouldReturnNotFoundWhenAjaxRequestAndInvalidMovieId()
        {
            // Arrange
            Mock<IMovieDbService> movieDbService = new Mock<IMovieDbService>();
            movieDbService
                .Setup(ms => ms.GetMovieDetailsAsync(It.IsAny<int>()))
                .ReturnsAsync((MovieDetailsServiceModel)null);

            HomeController controller = new HomeController(
                movieDbService.Object, null, null, null, null, null, null, null);
            controller.InjectAjaxRequest(true);

            // Act
            IActionResult actionResult = await controller.Index(InvalidMovieId);

            // Assert
            actionResult.Should().BeOfType<NotFoundResult>();
        }

        #endregion Index_ShouldReturnNotFoundWhenAjaxRequestAndInvalidMovieId

        #region Index_ShouldReturnPartialViewWhenAjaxRequestAndValidMovieId

        [Fact]
        public async Task Index_ShouldReturnPartialViewWhenAjaxRequestAndValidMovieId()
        {
            // Arrange
            Mock<IMovieDbService> movieDbService = new Mock<IMovieDbService>();
            movieDbService
                .Setup(ms => ms.GetMovieDetailsAsync(It.IsAny<int>()))
                .ReturnsAsync(new MovieDetailsServiceModel() { Id = FirstMovieId, Title = FirstMovieTitle, ProductionYear = FirstMovieProductionYear });

            HomeController controller = new HomeController(
                movieDbService.Object, null, null, null, null, null, null, null);
            controller.InjectAjaxRequest(true);

            // Act
            IActionResult actionResult = await controller.Index(FirstMovieId);

            // Assert
            actionResult.Should().BeOfType<PartialViewResult>();

            object model = actionResult.As<PartialViewResult>().Model;
            model.Should().BeOfType<MovieIndexViewModel>();

            MovieIndexViewModel returnedModel = model.As<MovieIndexViewModel>();
            returnedModel.MovieDetails.Id.Should().Be(FirstMovieId);
        }

        #endregion Index_ShouldReturnPartialViewWhenAjaxRequestAndValidMovieId

        #region Index_ShouldReturnCorrectViewModelWhenNotAjaxRequest

        [Fact]
        public async Task Index_ShouldReturnCorrectViewModelWhenNotAjaxRequest()
        {
            // Arrange
            var allMovies = new List<MovieBaseServiceModel>()
            {
                new MovieBaseServiceModel() { Id = FirstMovieId, ProductionYear = FirstMovieProductionYear, Title = FirstMovieTitle },
                new MovieBaseServiceModel() { Id = SecondMovieId, ProductionYear = SecondMovieProductionYear, Title = SecondMovieTitle },
                new MovieBaseServiceModel() { Id = ThirdMovieId, ProductionYear = ThirdMovieProductionYear, Title = ThirdMovieTitle }
            };

            Mock<IMovieDbService> movieDbService = new Mock<IMovieDbService>();
            movieDbService
                .Setup(ms => ms.GetAllMoviesAsync())
                .ReturnsAsync(allMovies);

            movieDbService
                .Setup(ms => ms.GetMovieDetailsAsync(It.IsAny<int>()))
                .ReturnsAsync(new MovieDetailsServiceModel() { Id = FirstMovieId, Title = FirstMovieTitle, ProductionYear = FirstMovieProductionYear });

            HomeController controller = new HomeController(
                movieDbService.Object, null, null, null, null, null, null, null);
            controller.InjectAjaxRequest(false);

            // Act
            IActionResult actionResult = await controller.Index(FirstMovieId);

            // Assert
            actionResult.Should().BeOfType<ViewResult>();

            object model = actionResult.As<ViewResult>().Model;
            model.Should().BeOfType<MovieIndexViewModel>();

            MovieIndexViewModel returnedModel = model.As<MovieIndexViewModel>();
            returnedModel.AllMovies.Should().Match(m => m.As<List<MovieBaseServiceModel>>().Count() == 3);
            returnedModel.MovieDetails.Should().Match(m => m.As<MovieDetailsServiceModel>().Id == FirstMovieId);
        }

        #endregion Index_ShouldReturnCorrectViewModelWhenNotAjaxRequest

        #endregion Index
    }
}
