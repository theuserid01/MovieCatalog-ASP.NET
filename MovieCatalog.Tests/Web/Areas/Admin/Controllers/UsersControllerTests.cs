namespace MovieCatalog.Tests.Web.Areas.Admin.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using MovieCatalog.Common;
    using MovieCatalog.Data.Models;
    using MovieCatalog.Services.Data.Contracts;
    using MovieCatalog.Services.Data.Models.Users;
    using MovieCatalog.Tests.Mocks;
    using MovieCatalog.Web.Areas.Admin.Controllers;
    using MovieCatalog.Web.Areas.Admin.Models.Users;
    using MovieCatalog.Web.Models.Account;
    using Xunit;

    public class UsersControllerTests
    {
        private const string FirstUserId = "1";
        private const string FirstUserUsername = "First";
        private const string FirstUserEmail = "first@gmail.com";
        private const string SecondUserId = "2";
        private const string SecondUserUsername = "Second";
        private const string SecondUserEmail = "second@gmail.com";
        private const string SearchTerm = "first";
        private const string InvalidUserId = null;
        private const string MessageDeleted = "User First has been successfully deleted.";
        private const string MessageRegistered = "User First has been successfully registered.";
        private const string MessageUpdated = "First profile has been successfully updated.";

        #region Attributes

        #region UsersController_ShouldBeInAdminArea

        [Fact]
        public void UsersController_ShouldBeInAdminArea()
        {
            // Arrange
            Type controller = typeof(UsersController);

            // Act
            AreaAttribute areaAttribute = controller
                .GetCustomAttributes(true)
                .FirstOrDefault(a => a.GetType() == typeof(AreaAttribute))
                as AreaAttribute;

            // Assert
            areaAttribute.Should().NotBeNull();
            areaAttribute.RouteValue.Should().Be(WebConstants.AdminArea);
        }

        #endregion UsersController_ShouldBeInAdminArea

        #region UsersController_ShouldOnlyBeForAdminUsers

        [Fact]
        public void UsersController_ShouldOnlyBeForAdminUsers()
        {
            // Arrange
            Type controller = typeof(UsersController);

            // Act
            AuthorizeAttribute authorizeAttribute = controller
                .GetCustomAttributes(true)
                .FirstOrDefault(a => a.GetType() == typeof(AuthorizeAttribute))
                as AuthorizeAttribute;

            // Assert
            authorizeAttribute.Should().NotBeNull();
            authorizeAttribute.Roles.Should().Be(GlobalConstants.AdministratorRole);
        }

        #endregion UsersController_ShouldOnlyBeForAdminUsers

        #endregion Attributes

        #region Index

        #region Index_ShouldReturnAllUsersWithSearchTermNUll

        [Fact]
        public async Task Index_ShouldReturnAllUsersWithSearchTermNUll()
        {
            // Arrange
            Mock<IAdminService> adminService = new Mock<IAdminService>();
            adminService
                .Setup(service => service.GetAllUsers(1, null))
                .Returns(new List<UserBaseServiceModel>()
                {
                    new UserBaseServiceModel() { Id = FirstUserId, Email = FirstUserEmail, Username = FirstUserUsername },
                    new UserBaseServiceModel() { Id = SecondUserId, Email = SecondUserEmail, Username = SecondUserUsername }
                });

            UsersController controller = new UsersController(adminService.Object, null, null);

            // Act
            IActionResult actionResult = await controller.Index(1, null);

            // Assert
            actionResult.Should().BeOfType<ViewResult>();

            object model = actionResult.As<ViewResult>().Model;
            model.Should().BeOfType<UsersPaginationModel>();

            UsersPaginationModel returnedModel = model.As<UsersPaginationModel>();
            returnedModel.Users.Should().Match(u => u.As<List<UserBaseServiceModel>>().Count() == 2);
            returnedModel.Users.First().Should().Match(u => u.As<UserBaseServiceModel>().Id == FirstUserId);
            returnedModel.Users.Last().Should().Match(u => u.As<UserBaseServiceModel>().Id == SecondUserId);
            returnedModel.Search.Should().BeNull();
        }

        #endregion Index_ShouldReturnAllUsersWithSearchTermNUll

        #region Index_ShouldReturnFilteredUsersWithSearchTermNotNull

        [Fact]
        public async Task Index_ShouldReturnFilteredUsersWithSearchTermNotNull()
        {
            // Arrange
            Mock<IAdminService> adminService = new Mock<IAdminService>();
            adminService
                .Setup(service => service.GetAllUsers(1, SearchTerm))
                .Returns(new List<UserBaseServiceModel>()
                {
                    new UserBaseServiceModel() { Id = FirstUserId, Email = FirstUserEmail, Username = FirstUserUsername },
                });

            UsersController controller = new UsersController(adminService.Object, null, null);

            // Act
            IActionResult actionResult = await controller.Index(1, SearchTerm);

            // Assert
            actionResult.Should().BeOfType<ViewResult>();

            object model = actionResult.As<ViewResult>().Model;
            model.Should().BeOfType<UsersPaginationModel>();

            UsersPaginationModel returnedModel = model.As<UsersPaginationModel>();
            returnedModel.Users.Should().Match(u => u.As<List<UserBaseServiceModel>>().Count() == 1);
            returnedModel.Users.First().Should().Match(u => u.As<UserBaseServiceModel>().Id == FirstUserId);
            returnedModel.Users.First().Should().Match(u => u.As<UserBaseServiceModel>().Username.ToLower().Contains(SearchTerm.ToLower()));
            returnedModel.Search.Should().Be(SearchTerm);
        }

        #endregion Index_ShouldReturnFilteredUsersWithSearchTermNotNull

        #endregion Index

        #region ChangeUserDetails

        #region ChangeUserDetailsGet_ShouldReturnNotFoundWithInvalidUsername

        [Fact]
        public async Task ChangeUserDetailsGet_ShouldReturnNotFoundWhenInvalidUserId()
        {
            // Arrange
            Mock<UserManager<User>> userManager = UserManagerMock.GetNew();

            UsersController controller = new UsersController(null, null, userManager.Object);

            // Act
            IActionResult actionResult = await controller.ChangeUserDetails(InvalidUserId, 1, null);

            // Assert
            actionResult.Should().BeOfType<NotFoundResult>();
        }

        #endregion ChangeUserDetailsGet_ShouldReturnNotFoundWithInvalidUsername

        #region ChangeUserDetailsGet_ShouldReturnViewWithCorrectModelWhenValidUserId

        [Fact]
        public async Task ChangeUserDetailsGet_ShouldReturnViewWithCorrectModelWhenValidUserId()
        {
            // Arrange
            Mock<UserManager<User>> userManager = this.GetAndSetUserManagerMock();

            UsersController controller = new UsersController(null, null, userManager.Object);

            // Act
            IActionResult actionResult = await controller.ChangeUserDetails(FirstUserId, 1, null);

            // Arrange
            actionResult.Should().BeOfType<ViewResult>();

            object model = actionResult.As<ViewResult>().Model;
            model.Should().BeOfType<ChangeUserDetailsFormModel>();

            ChangeUserDetailsFormModel returnedModel = model.As<ChangeUserDetailsFormModel>();
            returnedModel.Email.Should().Be(FirstUserEmail);
            returnedModel.Username.Should().Be(FirstUserUsername);
        }

        #endregion ChangeUserDetailsGet_ShouldReturnViewWithCorrectModelWhenValidUserId

        #region ChangeUserDetailsPost_ShouldReturnViewWithCorrectModelWhenModelStateIsInvalid

        [Fact]
        public async Task ChangeUserDetailsPost_ShouldReturnViewWithCorrectModelWhenModelStateIsInvalid()
        {
            // Arrange
            UsersController controller = new UsersController(null, null, null);
            controller.ModelState.AddModelError(string.Empty, "Error");

            // Act
            IActionResult actionResult = await controller.ChangeUserDetails(FirstUserId, new ChangeUserDetailsFormModel());

            // Assert
            actionResult.Should().BeOfType<ViewResult>();

            object model = actionResult.As<ViewResult>().Model;
            model.Should().BeOfType<ChangeUserDetailsFormModel>();
        }

        #endregion ChangeUserDetailsPost_ShouldReturnViewWithCorrectModelWhenModelStateIsInvalid

        #region ChangeUserDetailsPost_ShouldReturnNotFoundWhenInvalidUserId

        [Fact]
        public async Task ChangeUserDetailsPost_ShouldReturnNotFoundWhenInvalidUserId()
        {
            // Arrange
            Mock<UserManager<User>> userManager = UserManagerMock.GetNew();

            UsersController controller = new UsersController(null, null, userManager.Object);

            // Act
            IActionResult actionResult = await controller.ChangeUserDetails(InvalidUserId, new ChangeUserDetailsFormModel());

            // Assert
            actionResult.Should().BeOfType<NotFoundResult>();
        }

        #endregion ChangeUserDetailsPost_ShouldReturnNotFoundWhenInvalidUserId

        #region ChangeUserDetailsPost_ShouldReturnViewWithModelWhenSetEmailAsyncFailed

        [Fact]
        public async Task ChangeUserDetailsPost_ShouldReturnViewWithModelWhenSetEmailAsyncFailed()
        {
            // Arrange
            ChangeUserDetailsFormModel formModel = new ChangeUserDetailsFormModel()
            {
                Email = "NewEmail",
                Username = "NewUsername"
            };

            Mock<UserManager<User>> userManager = this.GetAndSetUserManagerMock();

            userManager
                .Setup(um => um.SetEmailAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "test" }));

            userManager
                .Setup(um => um.SetUserNameAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            UsersController controller = new UsersController(null, null, userManager.Object);

            // Act
            IActionResult actionResult = await controller.ChangeUserDetails(FirstUserId, formModel);

            // Assert
            actionResult.Should().BeOfType<ViewResult>();

            object model = actionResult.As<ViewResult>().Model;
            model.Should().BeOfType<ChangeUserDetailsFormModel>();

            ChangeUserDetailsFormModel returnedModel = model.As<ChangeUserDetailsFormModel>();
            returnedModel.Email.Should().Be(formModel.Email);
            returnedModel.Username.Should().Be(formModel.Username);
        }

        #endregion ChangeUserDetailsPost_ShouldReturnViewWithModelWhenSetEmailAsyncFailed

        #region ChangeUserDetailsPost_ShouldReturnViewWithModelWhenSetUsernameAsyncFailed

        [Fact]
        public async Task ChangeUserDetailsPost_ShouldReturnViewWithModelWhenSetUsernameAsyncFailed()
        {
            // Arrange
            ChangeUserDetailsFormModel formModel = new ChangeUserDetailsFormModel()
            {
                Email = "NewEmail",
                Username = "NewUsername"
            };

            Mock<UserManager<User>> userManager = this.GetAndSetUserManagerMock();

            userManager
                .Setup(um => um.SetEmailAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            userManager
                .Setup(um => um.SetUserNameAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "test" }));

            UsersController controller = new UsersController(null, null, userManager.Object);

            // Act
            IActionResult actionResult = await controller.ChangeUserDetails(FirstUserId, formModel);

            // Assert
            actionResult.Should().BeOfType<ViewResult>();

            object model = actionResult.As<ViewResult>().Model;
            model.Should().BeOfType<ChangeUserDetailsFormModel>();

            ChangeUserDetailsFormModel returnedModel = model.As<ChangeUserDetailsFormModel>();
            returnedModel.Email.Should().Be(formModel.Email);
            returnedModel.Username.Should().Be(formModel.Username);
        }

        #endregion ChangeUserDetailsPost_ShouldReturnViewWithModelWhenSetUsernameAsyncFailed

        #region ChangeUserDetailsPost_ShouldReturnViewWithCorrectModelWhenValidUserId

        [Fact]
        public async Task ChangeUserDetailsPost_ShouldReturnRedirectToActionWhenValidUserId()
        {
            // Arrange
            string tempDataSuccessMessage = null;

            ChangeUserDetailsFormModel formModel = new ChangeUserDetailsFormModel()
            {
                Email = "NewEmail",
                Username = "NewUsername"
            };

            Mock<UserManager<User>> userManager = this.GetAndSetUserManagerMock();

            userManager
                .Setup(um => um.SetEmailAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            userManager
                .Setup(um => um.SetUserNameAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            Mock<ITempDataDictionary> tempData = new Mock<ITempDataDictionary>();
            tempData.SetupSet(t => t[WebConstants.TempDataSuccessMessageKey] = It.IsAny<string>())
                .Callback((string key, object successMessage) => tempDataSuccessMessage = successMessage as string);

            UsersController controller = new UsersController(null, null, userManager.Object);
            controller.TempData = tempData.Object;

            // Act
            IActionResult actionResult = await controller.ChangeUserDetails(FirstUserId, formModel);

            // Assert
            actionResult.Should().BeOfType<RedirectToActionResult>();
            actionResult.As<RedirectToActionResult>().ActionName.Should().Be("Index");

            tempDataSuccessMessage.Should().Be(MessageUpdated);
        }

        #endregion ChangeUserDetailsPost_ShouldReturnViewWithCorrectModelWhenValidUserId

        #endregion ChangeUserDetails

        #region ChangeUserPassword

        #region ChangeUserPasswordGet_ShouldReturnNotFoundWhenInvalidUserId

        [Fact]
        public async Task ChangeUserPasswordGet_ShouldReturnNotFoundWhenInvalidUserId()
        {
            // Arrange
            Mock<UserManager<User>> userManager = UserManagerMock.GetNew();

            UsersController controller = new UsersController(null, null, userManager.Object);

            // Act
            IActionResult actionResult = await controller.ChangeUserPassword(InvalidUserId, 1, null);

            // Assert
            actionResult.Should().BeOfType<NotFoundResult>();
        }

        #endregion ChangeUserPasswordGet_ShouldReturnNotFoundWhenInvalidUserId

        #region ChangeUserPasswordGet_ShouldReturnViewWithCorrectModelWhenValidUserId

        [Fact]
        public async Task ChangeUserPasswordGet_ShouldReturnViewWithCorrectModelWhenValidUserId()
        {
            // Arrange
            Mock<UserManager<User>> userManager = this.GetAndSetUserManagerMock();

            UsersController controller = new UsersController(null, null, userManager.Object);

            // Act
            IActionResult actionResult = await controller.ChangeUserPassword(FirstUserId, 1, null);

            // Arrange
            actionResult.Should().BeOfType<ViewResult>();

            object model = actionResult.As<ViewResult>().Model;
            model.Should().BeOfType<ChangeUserPasswordFormModel>();

            ChangeUserPasswordFormModel returnedModel = model.As<ChangeUserPasswordFormModel>();
            returnedModel.Username.Should().Be(FirstUserUsername);
        }

        #endregion ChangeUserPasswordGet_ShouldReturnViewWithCorrectModelWhenValidUserId

        #region ChangeUserPasswordPost_ShouldReturnViewWithCorrectModelWhenModelStateIsInvalid

        [Fact]
        public async Task ChangeUserPasswordPost_ShouldReturnViewWithCorrectModelWhenModelStateIsInvalid()
        {
            // Arrange
            UsersController controller = new UsersController(null, null, null);
            controller.ModelState.AddModelError(string.Empty, "Error");

            // Act
            IActionResult actionResult = await controller.ChangeUserPassword(FirstUserId, new ChangeUserPasswordFormModel());

            // Assert
            actionResult.Should().BeOfType<ViewResult>();

            object model = actionResult.As<ViewResult>().Model;
            model.Should().BeOfType<ChangeUserPasswordFormModel>();
        }

        #endregion ChangeUserPasswordPost_ShouldReturnViewWithCorrectModelWhenModelStateIsInvalid

        #region ChangeUserPasswordPost_ShouldReturnNotFoundWhenInvalidUserId

        [Fact]
        public async Task ChangeUserPasswordPost_ShouldReturnNotFoundWhenInvalidUserId()
        {
            // Arrange
            Mock<UserManager<User>> userManager = UserManagerMock.GetNew();

            UsersController controller = new UsersController(null, null, userManager.Object);

            // Act
            IActionResult actionResult = await controller.ChangeUserPassword(InvalidUserId, new ChangeUserPasswordFormModel());

            // Assert
            actionResult.Should().BeOfType<NotFoundResult>();
        }

        #endregion ChangeUserPasswordPost_ShouldReturnNotFoundWhenInvalidUserId

        #region ChangeUserPasswordPost_ShouldReturnViewWithModelWhenResetPasswordAsyncFailed

        [Fact]
        public async Task ChangeUserPasswordPost_ShouldReturnViewWithModelWhenResetPasswordAsyncFailed()
        {
            // Arrange
            ChangeUserPasswordFormModel formModel = new ChangeUserPasswordFormModel()
            {
                Username = FirstUserUsername
            };

            Mock<UserManager<User>> userManager = this.GetAndSetUserManagerMock();

            userManager
                .Setup(um => um.ResetPasswordAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "test" }));

            UsersController controller = new UsersController(null, null, userManager.Object);

            // Act
            IActionResult actionResult = await controller.ChangeUserPassword(FirstUserId, formModel);

            // Assert
            actionResult.Should().BeOfType<ViewResult>();

            object model = actionResult.As<ViewResult>().Model;
            model.Should().BeOfType<ChangeUserPasswordFormModel>();

            ChangeUserPasswordFormModel returnedModel = model.As<ChangeUserPasswordFormModel>();
            returnedModel.Username.Should().Be(formModel.Username);
        }

        #endregion ChangeUserPasswordPost_ShouldReturnViewWithModelWhenResetPasswordAsyncFailed

        #region ChangeUserPasswordPost_ShouldReturnRedirectToActionWhenValidUserId

        [Fact]
        public async Task ChangeUserPasswordPost_ShouldReturnRedirectToActionWhenValidUserId()
        {
            // Arrange
            string tempDataSuccessMessage = null;

            ChangeUserPasswordFormModel formModel = new ChangeUserPasswordFormModel()
            {
                Username = FirstUserUsername
            };

            Mock<UserManager<User>> userManager = this.GetAndSetUserManagerMock();

            userManager
                .Setup(um => um.ResetPasswordAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            Mock<ITempDataDictionary> tempData = new Mock<ITempDataDictionary>();
            tempData.SetupSet(t => t[WebConstants.TempDataSuccessMessageKey] = It.IsAny<string>())
                .Callback((string key, object successMessage) => tempDataSuccessMessage = successMessage as string);

            UsersController controller = new UsersController(null, null, userManager.Object);
            controller.TempData = tempData.Object;

            // Act
            IActionResult actionResult = await controller.ChangeUserPassword(FirstUserId, formModel);

            // Assert
            actionResult.Should().BeOfType<RedirectToActionResult>();
            actionResult.As<RedirectToActionResult>().ActionName.Should().Be("Index");

            tempDataSuccessMessage.Should().Be(MessageUpdated);
        }

        #endregion ChangeUserPasswordPost_ShouldReturnRedirectToActionWhenValidUserId

        #endregion ChangeUserPassword

        #region ChangeUserRoles

        #region ChangeUserRolesGet_ShouldReturnNotFoundWhenInvalidUserId

        [Fact]
        public async Task ChangeUserRolesGet_ShouldReturnNotFoundWhenInvalidUserId()
        {
            // Arrange
            Mock<UserManager<User>> userManager = UserManagerMock.GetNew();

            UsersController controller = new UsersController(null, null, userManager.Object);

            // Act
            IActionResult actionResult = await controller.ChangeUserRoles(InvalidUserId, 1, null);

            // Assert
            actionResult.Should().BeOfType<NotFoundResult>();
        }

        #endregion ChangeUserRolesGet_ShouldReturnNotFoundWhenInvalidUserId

        #region ChangeUserRolesGet_ShouldReturnViewWithCorrectModelWhenValidUserId

        [Fact]
        public async Task ChangeUserRolesGet_ShouldReturnViewWithCorrectModelWhenValidUserId()
        {
            // Arrange
            Mock<RoleManager<IdentityRole>> roleManager = GetAndSetRoleManagerMock();

            Mock<UserManager<User>> userManager = GetAndSetUserManagerMock();

            UsersController controller = new UsersController(null, roleManager.Object, userManager.Object);

            // Act
            IActionResult actionResult = await controller.ChangeUserRoles(FirstUserId, 1, null);

            // Arrange
            actionResult.Should().BeOfType<ViewResult>();

            object model = actionResult.As<ViewResult>().Model;
            model.Should().BeOfType<ChangeUserRolesFormModel>();

            ChangeUserRolesFormModel returnedModel = model.As<ChangeUserRolesFormModel>();
            returnedModel.Username.Should().Be(FirstUserUsername);
        }

        #endregion ChangeUserRolesGet_ShouldReturnViewWithCorrectModelWhenValidUserId

        #region ChangeUserRolesPost_ShouldReturnNotFoundWhenInvalidUserId

        [Fact]
        public async Task ChangeUserRolesPost_ShouldReturnNotFoundWhenInvalidUserId()
        {
            // Arrange
            Mock<UserManager<User>> userManager = UserManagerMock.GetNew();
            UsersController controller = new UsersController(null, null, userManager.Object);

            // Act
            IActionResult actionResult = await controller.ChangeUserRoles(InvalidUserId, new ChangeUserRolesFormModel());

            // Assert
            actionResult.Should().BeOfType<NotFoundResult>();
        }

        #endregion ChangeUserRolesPost_ShouldReturnNotFoundWhenInvalidUserId

        #region ChangeUserRolesPost_ShouldReturnBadRequestWhenAvailableRolesCountIsZeroAndSelectedRolesCountIsZero

        [Fact]
        public async Task ChangeUserRolesPost_ShouldReturnBadRequestWhenAvailableRolesCountIsZeroAndSelectedRolesCountIsZero()
        {
            // Arrange
            ChangeUserRolesFormModel formModel = new ChangeUserRolesFormModel()
            {
                AvailableRoles = new List<string>(),
                SelectedRoles = new List<string>()
            };

            Mock<UserManager<User>> userManager = this.GetAndSetUserManagerMock();

            UsersController controller = new UsersController(null, null, userManager.Object);

            // Act
            IActionResult actionResult = await controller.ChangeUserRoles(FirstUserId, formModel);

            // Assert
            actionResult.Should().BeOfType<BadRequestResult>();
        }

        #endregion ChangeUserRolesPost_ShouldReturnBadRequestWhenAvailableRolesCountIsZeroAndSelectedRolesCountIsZero

        #region ChangeUserRolesPost_ShouldReturnBadRequestWhenSelectedRoleNotExist

        [Fact]
        public async Task ChangeUserRolesPost_ShouldReturnBadRequestWhenSelectedRoleNotExist()
        {
            // Arrange
            ChangeUserRolesFormModel formModel = new ChangeUserRolesFormModel()
            {
                AvailableRoles = new List<string>(),
                SelectedRoles = new List<string>() { "InvalidRole" },
                Username = FirstUserUsername
            };

            Mock<RoleManager<IdentityRole>> roleManager = this.GetAndSetRoleManagerMock();
            roleManager
                .Setup(rm => rm.RoleExistsAsync(It.IsAny<string>()))
                .ReturnsAsync(false);

            Mock<UserManager<User>> userManager = this.GetAndSetUserManagerMock();

            UsersController controller = new UsersController(null, roleManager.Object, userManager.Object);

            // Act
            IActionResult actionResult = await controller.ChangeUserRoles(FirstUserId, formModel);

            // Assert
            actionResult.Should().BeOfType<BadRequestResult>();
        }

        #endregion ChangeUserRolesPost_ShouldReturnBadRequestWhenSelectedRoleNotExist

        #region ChangeUserRolesPost_ShouldReturnViewWithModelWhenRemoveFromRolesAsyncFailed

        [Fact]
        public async Task ChangeUserRolesPost_ShouldReturnViewWithModelWhenRemoveFromRolesAsyncFailed()
        {
            // Arrange
            ChangeUserRolesFormModel formModel = new ChangeUserRolesFormModel()
            {
                AvailableRoles = new List<string>(),
                SelectedRoles = new List<string>() { GlobalConstants.ModeratorRole },
                Username = FirstUserUsername
            };

            Mock<RoleManager<IdentityRole>> roleManager = this.GetAndSetRoleManagerMock();
            roleManager
                .Setup(rm => rm.RoleExistsAsync(It.IsAny<string>()))
                .ReturnsAsync(true);

            Mock<UserManager<User>> userManager = this.GetAndSetUserManagerMock();
            userManager
                .Setup(um => um.RemoveFromRolesAsync(It.IsAny<User>(), It.IsAny<List<string>>()))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "test" }));

            UsersController controller = new UsersController(null, roleManager.Object, userManager.Object);

            // Act
            IActionResult actionResult = await controller.ChangeUserRoles(FirstUserId, formModel);

            // Assert
            actionResult.Should().BeOfType<ViewResult>();

            object model = actionResult.As<ViewResult>().Model;
            model.Should().BeOfType<ChangeUserRolesFormModel>();

            ChangeUserRolesFormModel returnedModel = model.As<ChangeUserRolesFormModel>();
            returnedModel.Username.Should().Be(formModel.Username);
        }

        #endregion ChangeUserRolesPost_ShouldReturnViewWithModelWhenRemoveFromRolesAsyncFailed

        #region ChangeUserRolesPost_ShouldReturnRedirectToActionWhenSelectRolesCountIsZero

        [Fact]
        public async Task ChangeUserRolesPost_ShouldReturnRedirectToActionWhenSelectRolesCountIsZero()
        {
            // Arrange
            string tempDataSuccessMessage = null;

            ChangeUserRolesFormModel formModel = new ChangeUserRolesFormModel()
            {
                AvailableRoles = new List<string>() { GlobalConstants.ModeratorRole },
                SelectedRoles = new List<string>(),
                Username = FirstUserUsername
            };

            Mock<RoleManager<IdentityRole>> roleManager = this.GetAndSetRoleManagerMock();
            roleManager
                .Setup(rm => rm.RoleExistsAsync(It.IsAny<string>()))
                .ReturnsAsync(true);

            Mock<UserManager<User>> userManager = this.GetAndSetUserManagerMock();
            userManager
                .Setup(um => um.RemoveFromRolesAsync(It.IsAny<User>(), It.IsAny<List<string>>()))
                .ReturnsAsync(IdentityResult.Success);

            Mock<ITempDataDictionary> tempData = new Mock<ITempDataDictionary>();
            tempData.SetupSet(t => t[WebConstants.TempDataSuccessMessageKey] = It.IsAny<string>())
                .Callback((string key, object successMessage) => tempDataSuccessMessage = successMessage as string);

            UsersController controller = new UsersController(null, roleManager.Object, userManager.Object);
            controller.TempData = tempData.Object;

            // Act
            IActionResult actionResult = await controller.ChangeUserRoles(FirstUserId, formModel);

            // Assert
            actionResult.Should().BeOfType<RedirectToActionResult>();
            actionResult.As<RedirectToActionResult>().ActionName.Should().Be("Index");

            tempDataSuccessMessage.Should().Be(MessageUpdated);
        }

        #endregion ChangeUserRolesPost_ShouldReturnRedirectToActionWhenSelectRolesCountIsZero

        #region ChangeUserRolesPost_ShouldReturnViewWithModelWhenAddToRolesAsyncFailed

        [Fact]
        public async Task ChangeUserRolesPost_ShouldReturnViewWithModelWhenAddToRolesAsyncFailed()
        {
            // Arrange
            ChangeUserRolesFormModel formModel = new ChangeUserRolesFormModel()
            {
                AvailableRoles = new List<string>(),
                SelectedRoles = new List<string>() { GlobalConstants.ModeratorRole },
                Username = FirstUserUsername
            };

            Mock<RoleManager<IdentityRole>> roleManager = this.GetAndSetRoleManagerMock();
            roleManager
                .Setup(rm => rm.RoleExistsAsync(It.IsAny<string>()))
                .ReturnsAsync(true);

            Mock<UserManager<User>> userManager = this.GetAndSetUserManagerMock();
            userManager
                .Setup(um => um.RemoveFromRolesAsync(It.IsAny<User>(), It.IsAny<List<string>>()))
                .ReturnsAsync(IdentityResult.Success);

            userManager
               .Setup(um => um.AddToRolesAsync(It.IsAny<User>(), It.IsAny<List<string>>()))
               .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "test" }));

            UsersController controller = new UsersController(null, roleManager.Object, userManager.Object);

            // Act
            IActionResult actionResult = await controller.ChangeUserRoles(FirstUserId, formModel);

            // Assert
            actionResult.Should().BeOfType<ViewResult>();

            object model = actionResult.As<ViewResult>().Model;
            model.Should().BeOfType<ChangeUserRolesFormModel>();

            ChangeUserRolesFormModel returnedModel = model.As<ChangeUserRolesFormModel>();
            returnedModel.Username.Should().Be(formModel.Username);
        }

        #endregion ChangeUserRolesPost_ShouldReturnViewWithModelWhenAddToRolesAsyncFailed

        #region ChangeUserRolesPost_ShouldReturnRedirectToActionWhenValidUserId

        [Fact]
        public async Task ChangeUserRolesPost_ShouldReturnRedirectToActionWhenValidUserId()
        {
            // Arrange
            string tempDataSuccessMessage = null;

            ChangeUserRolesFormModel formModel = new ChangeUserRolesFormModel()
            {
                AvailableRoles = new List<string>(),
                SelectedRoles = new List<string>() { GlobalConstants.ModeratorRole },
                Username = FirstUserUsername
            };

            Mock<RoleManager<IdentityRole>> roleManager = this.GetAndSetRoleManagerMock();
            roleManager
                .Setup(rm => rm.RoleExistsAsync(It.IsAny<string>()))
                .ReturnsAsync(true);

            Mock<UserManager<User>> userManager = this.GetAndSetUserManagerMock();
            userManager
                .Setup(um => um.RemoveFromRolesAsync(It.IsAny<User>(), It.IsAny<List<string>>()))
                .ReturnsAsync(IdentityResult.Success);

            userManager
              .Setup(um => um.AddToRolesAsync(It.IsAny<User>(), It.IsAny<List<string>>()))
              .ReturnsAsync(IdentityResult.Success);

            Mock<ITempDataDictionary> tempData = new Mock<ITempDataDictionary>();
            tempData.SetupSet(t => t[WebConstants.TempDataSuccessMessageKey] = It.IsAny<string>())
                .Callback((string key, object successMessage) => tempDataSuccessMessage = successMessage as string);

            UsersController controller = new UsersController(null, roleManager.Object, userManager.Object);
            controller.TempData = tempData.Object;

            // Act
            IActionResult actionResult = await controller.ChangeUserRoles(FirstUserId, formModel);

            // Assert
            actionResult.Should().BeOfType<RedirectToActionResult>();
            actionResult.As<RedirectToActionResult>().ActionName.Should().Be("Index");

            tempDataSuccessMessage.Should().Be(MessageUpdated);
        }

        #endregion ChangeUserRolesPost_ShouldReturnRedirectToActionWhenValidUserId

        #endregion ChangeUserRoles

        #region DeleteUser

        #region DeleteUserGet_ShouldReturnNotFoundWhenInvalidUserId

        [Fact]
        public async Task DeleteUserGet_ShouldReturnNotFoundWhenInvalidUserId()
        {
            // Arrange
            Mock<UserManager<User>> userManager = UserManagerMock.GetNew();

            UsersController controller = new UsersController(null, null, userManager.Object);

            // Act
            IActionResult actionResult = await controller.DeleteUser(InvalidUserId, 1, null);

            // Assert
            actionResult.Should().BeOfType<NotFoundResult>();
        }

        #endregion DeleteUserGet_ShouldReturnNotFoundWhenInvalidUserId

        #region DeleteUserGet_ShouldReturnViewWithCorrectModelWhenValidUserId

        [Fact]
        public async Task DeleteUserGet_ShouldReturnViewWithCorrectModelWhenValidUserId()
        {
            // Arrange
            Mock<UserManager<User>> userManager = this.GetAndSetUserManagerMock();

            UsersController controller = new UsersController(null, null, userManager.Object);

            // Act
            IActionResult actionResult = await controller.DeleteUser(FirstUserId, 1, null);

            // Arrange
            actionResult.Should().BeOfType<ViewResult>();

            object model = actionResult.As<ViewResult>().Model;
            model.Should().BeOfType<DeleteUserFormModel>();

            DeleteUserFormModel returnedModel = model.As<DeleteUserFormModel>();
            returnedModel.Username.Should().Be(FirstUserUsername);
        }

        #endregion DeleteUserGet_ShouldReturnViewWithCorrectModelWhenValidUserId

        #region DeleteUserPost_ShouldReturnNotFoundWhenInvalidUserId

        [Fact]
        public async Task DeleteUserPost_ShouldReturnNotFoundWhenInvalidUserId()
        {
            // Arrange
            Mock<UserManager<User>> userManager = UserManagerMock.GetNew();

            UsersController controller = new UsersController(null, null, userManager.Object);

            // Act
            IActionResult actionResult = await controller.DeleteUser(InvalidUserId, new DeleteUserFormModel());

            // Assert
            actionResult.Should().BeOfType<NotFoundResult>();
        }

        #endregion DeleteUserPost_ShouldReturnNotFoundWhenInvalidUserId

        #region DeleteUserPost_ShouldReturnViewWithModelWhenDeleteAsyncFailed

        [Fact]
        public async Task DeleteUserPost_ShouldReturnViewWithModelWhenDeleteAsyncFailed()
        {
            // Arrange
            DeleteUserFormModel formModel = new DeleteUserFormModel()
            {
                Username = FirstUserUsername
            };

            Mock<UserManager<User>> userManager = this.GetAndSetUserManagerMock();

            userManager
                .Setup(um => um.DeleteAsync(It.IsAny<User>()))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "test" }));

            UsersController controller = new UsersController(null, null, userManager.Object);

            // Act
            IActionResult actionResult = await controller.DeleteUser(FirstUserId, formModel);

            // Assert
            actionResult.Should().BeOfType<ViewResult>();

            object model = actionResult.As<ViewResult>().Model;
            model.Should().BeOfType<DeleteUserFormModel>();

            var returnedModel = model.As<DeleteUserFormModel>();
            returnedModel.Username.Should().Be(formModel.Username);
        }

        #endregion DeleteUserPost_ShouldReturnViewWithModelWhenDeleteAsyncFailed

        #region DeleteUserPost_ShouldReturnRedirectToActionWhenValidUserId

        [Fact]
        public async Task DeleteUserPost_ShouldReturnRedirectToActionWhenValidUserId()
        {
            // Arrange
            string tempDataSuccessMessage = null;

            DeleteUserFormModel formModel = new DeleteUserFormModel()
            {
                Username = FirstUserUsername
            };

            Mock<UserManager<User>> userManager = this.GetAndSetUserManagerMock();

            userManager
                .Setup(um => um.DeleteAsync(It.IsAny<User>()))
                .ReturnsAsync(IdentityResult.Success);

            Mock<ITempDataDictionary> tempData = new Mock<ITempDataDictionary>();
            tempData.SetupSet(t => t[WebConstants.TempDataSuccessMessageKey] = It.IsAny<string>())
                .Callback((string key, object successMessage) => tempDataSuccessMessage = successMessage as string);

            UsersController controller = new UsersController(null, null, userManager.Object);
            controller.TempData = tempData.Object;

            // Act
            IActionResult actionResult = await controller.DeleteUser(FirstUserId, formModel);

            // Assert
            actionResult.Should().BeOfType<RedirectToActionResult>();
            actionResult.As<RedirectToActionResult>().ActionName.Should().Be("Index");

            tempDataSuccessMessage.Should().Be(MessageDeleted);
        }

        #endregion DeleteUserPost_ShouldReturnRedirectToActionWhenValidUserId

        #endregion DeleteUser

        #region RegisterUser

        #region RegisterUserGet_ShouldReturnView

        [Fact]
        public void RegisterUserGet_ShouldReturnView()
        {
            // Arrange
            UsersController controller = new UsersController(null, null, null);

            // Act
            IActionResult actionResult = controller.RegisterUser();

            // Assert
            actionResult.Should().BeOfType<ViewResult>();
        }

        #endregion RegisterUserGet_ShouldReturnView

        #region RegisterUserPost_ShouldReturnViewWithCorrectModelWhenModelStateIsInvalid

        [Fact]
        public async Task RegisterUserPost_ShouldReturnViewWithCorrectModelWhenModelStateIsInvalid()
        {
            // Arrange
            UsersController controller = new UsersController(null, null, null);
            controller.ModelState.AddModelError(string.Empty, "Error");

            // Act
            IActionResult actionResult = await controller.RegisterUser(new RegisterViewModel());

            // Assert
            actionResult.Should().BeOfType<ViewResult>();

            object model = actionResult.As<ViewResult>().Model;
            model.Should().BeOfType<RegisterViewModel>();
        }

        #endregion RegisterUserPost_ShouldReturnViewWithCorrectModelWhenModelStateIsInvalid

        #region RegisterUserPost_ShouldReturnViewWithModelWhenCreateAsyncFailed

        [Fact]
        public async Task RegisterUserPost_ShouldReturnViewWithModelWhenCreateAsyncFailed()
        {
            // Arrange
            RegisterViewModel formModel = new RegisterViewModel()
            {
                Username = FirstUserUsername
            };

            Mock<UserManager<User>> userManager = UserManagerMock.GetNew();

            userManager
                .Setup(x => x.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "test" }));

            UsersController controller = new UsersController(null, null, userManager.Object);

            // Act
            IActionResult actionResult = await controller.RegisterUser(formModel);

            // Assert
            actionResult.Should().BeOfType<ViewResult>();

            object model = actionResult.As<ViewResult>().Model;
            model.Should().BeOfType<RegisterViewModel>();

            var returnedModel = model.As<RegisterViewModel>();
            returnedModel.Username.Should().Be(formModel.Username);
        }

        #endregion RegisterUserPost_ShouldReturnViewWithModelWhenCreateAsyncFailed

        #region RegisterUserPost_ShouldReturnRedirectToActionWhenModelStateIsValid

        [Fact]
        public async Task RegisterUserPost_ShouldReturnRedirectToActionWhenModelStateIsValid()
        {
            // Arrange
            string tempDataSuccessMessage = null;

            RegisterViewModel formModel = new RegisterViewModel()
            {
                Email = FirstUserEmail,
                Username = FirstUserUsername,
            };

            Mock<UserManager<User>> userManager = UserManagerMock.GetNew();
            userManager
                .Setup(x => x.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            Mock<ITempDataDictionary> tempData = new Mock<ITempDataDictionary>();
            tempData.SetupSet(t => t[WebConstants.TempDataSuccessMessageKey] = It.IsAny<string>())
                .Callback((string key, object successMessage) => tempDataSuccessMessage = successMessage as string);

            UsersController controller = new UsersController(null, null, userManager.Object);
            controller.TempData = tempData.Object;

            // Act
            IActionResult actionResult = await controller.RegisterUser(formModel);

            // Assert
            actionResult.Should().BeOfType<RedirectToActionResult>();
            actionResult.As<RedirectToActionResult>().ActionName.Should().Be("Index");

            tempDataSuccessMessage.Should().Be(MessageRegistered);
        }

        #endregion RegisterUserPost_ShouldReturnRedirectToActionWhenModelStateIsValid

        #endregion RegisterUser

        #region Private Methods

        private Mock<RoleManager<IdentityRole>> GetAndSetRoleManagerMock()
        {
            Mock<RoleManager<IdentityRole>> roleManager = RoleManagerMock.GetNew();

            var roles = new List<IdentityRole>()
            {
                new IdentityRole(GlobalConstants.ModeratorRole)
            };

            Mock<DbSet<IdentityRole>> mockAsync = roles.ToDbSetAsyncMock();

            roleManager
                .Setup(rm => rm.Roles)
                .Returns(mockAsync.Object);

            return roleManager;
        }

        private Mock<UserManager<User>> GetAndSetUserManagerMock()
        {
            Mock<UserManager<User>> userManager = UserManagerMock.GetNew();

            User user = this.GetUser();

            userManager
                .Setup(um => um.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(user);

            userManager
                .Setup(um => um.GetRolesAsync(user))
                .ReturnsAsync(new List<string>() { GlobalConstants.AdministratorRole });

            return userManager;
        }

        private User GetUser()
        {
            return new User()
            {
                Id = FirstUserId,
                Email = FirstUserEmail,
                UserName = FirstUserUsername
            };
        }

        #endregion Private Methods
    }
}
