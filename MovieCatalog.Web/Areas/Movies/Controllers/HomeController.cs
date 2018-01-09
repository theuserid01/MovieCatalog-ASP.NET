namespace MovieCatalog.Web.Areas.Movies.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using MovieCatalog.Common;
    using MovieCatalog.Common.Extensions;
    using MovieCatalog.Services.Data.Contracts;
    using MovieCatalog.Services.Data.Models.Movies;
    using MovieCatalog.Services.Html.Contracts;
    using MovieCatalog.Services.Html.Models;
    using MovieCatalog.Services.Html.Models.BoxOfficeMojo;
    using MovieCatalog.Services.Html.Models.Imdb;
    using MovieCatalog.Services.Html.Models.RottenTomatoes;
    using MovieCatalog.Web.Areas.Movies.Models;
    using MovieCatalog.Web.Infrastructure.Extensions;

    public class HomeController : BaseController
    {
        private const string MovieCreatedSuccessMessage = "Movie {0} has been successfully created.";
        private const string MovieDeletedSuccessMessage = "Movie <em>{0}</em> has been successfully deleted.";
        private const string MovieEditedSuccessMessage = "Movie {0} has been successfully edited.";

        private readonly IMovieDbService movieDbService;
        private readonly IBlurayDotComService blurayDotComService;
        private readonly IBoxOfficeMojoService boxOfficeMojoService;
        private readonly IDvdEmpireService dvdEmpireService;
        private readonly IHtmlService htmlService;
        private readonly IImdbService imdbService;
        private readonly IMapper mapper;
        private readonly IRottenTomatoesService rottenTomatoesService;
        private IList<byte[]> posters;

        #region Constructor

        public HomeController(
            IMovieDbService movieDbService,
            IBlurayDotComService blurayDotComService,
            IBoxOfficeMojoService boxOfficeMojoService,
            IDvdEmpireService dvdEmpireService,
            IHtmlService htmlService,
            IImdbService imdbService,
            IMapper mapper,
            IRottenTomatoesService rottenTomatoesService)
        {
            this.movieDbService = movieDbService;
            this.blurayDotComService = blurayDotComService;
            this.boxOfficeMojoService = boxOfficeMojoService;
            this.dvdEmpireService = dvdEmpireService;
            this.htmlService = htmlService;
            this.imdbService = imdbService;
            this.mapper = mapper;
            this.rottenTomatoesService = rottenTomatoesService;
        }

        #endregion Constructor

        #region Index

        public async Task<IActionResult> Index(int id)
        {
            MovieDetailsServiceModel movieDetailsServiceModel = new MovieDetailsServiceModel();
            MovieIndexViewModel viewModel = new MovieIndexViewModel();

            bool isAjaxRequest = HttpContext.Request.IsAjaxRequest();

            if (isAjaxRequest)
            {
                movieDetailsServiceModel = await this.movieDbService.GetMovieDetailsAsync(id);

                if (movieDetailsServiceModel == null)
                {
                    return NotFound();
                }

                viewModel.MovieDetails = movieDetailsServiceModel;

                return PartialView("_Details", viewModel);
            }

            IEnumerable<MovieBaseServiceModel> allMovies
                = await this.movieDbService.GetAllMoviesAsync();

            if (allMovies != null && allMovies.Count() > 0)
            {
                int movieId = allMovies.FirstOrDefault().Id;
                movieDetailsServiceModel = await this.movieDbService.GetMovieDetailsAsync(movieId);

                viewModel.AllMovies = allMovies.AsNotNull();
                viewModel.MovieDetails = movieDetailsServiceModel;
            }

            viewModel.AllMovies = allMovies.AsNotNull();

            return View(viewModel);
        }

        #endregion Index

        #region Create HttpGet

        public async Task<IActionResult> Create()
        {
            var formModel = new MovieFormMainModel()
            {
                ProductionYear = DateTime.UtcNow.Year
            };

            await this.PopulateFormMainModel(formModel);

            return View(formModel);
        }

        #endregion Create HttpGet

        #region Create HttpPost

        [HttpPost]
        public async Task<IActionResult> Create(MovieFormMainModel formModel, List<IFormFile> files)
        {
            bool isValid = await this.ValidateFormModelAndFiles(formModel, files);

            if (!isValid)
            {
                return View(formModel);
            }

            foreach (var a in formModel.Crew)
            {
                var b = a.Name;
                var c = a.Role;
                var d = $"{b} {c}";
            }

            List<CastServiceModel> castArtists = this.mapper
                .Map<IEnumerable<CastFormModel>, List<CastServiceModel>>(formModel.Cast);

            List<CrewServiceModel> crewArtists = this.mapper
                .Map<IEnumerable<CrewFormModel>, List<CrewServiceModel>>(formModel.Crew);

            byte[] thumbnail = await this.GetThumbnail();

            try
            {
                await this.movieDbService.CreateMovieAsync(
                    formModel.BoxOfficeMojoId,
                    formModel.Budget,
                    castArtists.AsNotNull(),
                    crewArtists.AsNotNull(),
                    formModel.GrossForeign,
                    formModel.GrossUsa,
                    formModel.ImdbId,
                    formModel.ImdbTop250,
                    formModel.ImdbUsersRating,
                    formModel.OriginalTitle,
                    posters,
                    formModel.ProductionYear,
                    formModel.RottenTomatoesCriticsScore,
                    formModel.RottenTomatoesId,
                    formModel.RottenTomatoesUsersScore,
                    formModel.Runtime,
                    formModel.SelectedColors.AsNotNull(),
                    formModel.SelectedCountries.AsNotNull(),
                    formModel.SelectedGenres.AsNotNull(),
                    formModel.SelectedLanguages.AsNotNull(),
                    formModel.Synopsis,
                    thumbnail,
                    formModel.Title);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);

                await this.PopulateFormMainModel(formModel);

                return View(formModel);
            }

            TempData[WebConstants.TempDataSuccessMessageKey]
                = string.Format(MovieCreatedSuccessMessage, formModel.Title);

            return RedirectToAction(nameof(Index));
        }

        #endregion Create HttpPost

        #region Delete HttpGet

        [Authorize(Roles = GlobalConstants.AdministratorRole)]
        public async Task<IActionResult> Delete(int id, string title)
        {
            bool isAjaxRequest = HttpContext.Request.IsAjaxRequest();

            if (isAjaxRequest)
            {
                if (id < 1 || title == null)
                {
                    return BadRequest();
                }

                bool success = await this.movieDbService.DeleteMovieAsync(id);

                if (!success)
                {
                    return NotFound();
                }

                JsonResult json = Json(new
                {
                    message = string.Format(MovieDeletedSuccessMessage, title)
                });

                return json;
            }

            return RedirectToAction(nameof(Index));
        }

        #endregion Delete HttpGet

        #region Edit HttpGet

        public async Task<IActionResult> Edit(int id)
        {
            MovieDetailsServiceModel movieDetails
                = await this.movieDbService.GetMovieDetailsAsync(id);

            if (movieDetails == null)
            {
                return NotFound();
            }

            MovieFormMainModel formModel = new MovieFormMainModel();

            IMapper config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<MovieDetailsServiceModel, MovieFormMainModel>()
                    .ForMember(dest => dest.Cast, opts => opts
                        .MapFrom(source => source.Cast.Select(c =>
                            new CastFormModel() { ImdbId = c.ImdbId, Name = c.Name, Character = c.Character })))
                    .ForMember(dest => dest.Crew, opts => opts
                        .MapFrom(source => source.Crew.Select(c =>
                            new CrewFormModel() { ImdbId = c.ImdbId, Name = c.Name, Role = c.Role })))
                    .ForMember(dest => dest.SelectedColors, opts => opts
                        .MapFrom(source => source.Colors.Select(c => c.Id)))
                    .ForMember(dest => dest.SelectedCountries, opts => opts
                        .MapFrom(source => source.Countries.Select(c => c.Id)))
                    .ForMember(dest => dest.SelectedGenres, opts => opts
                        .MapFrom(source => source.Genres.Select(g => g.Id)))
                    .ForMember(dest => dest.SelectedLanguages, opts => opts
                        .MapFrom(source => source.Languages.Select(l => l.Id)));
            })
            .CreateMapper();

            formModel = config.Map<MovieDetailsServiceModel, MovieFormMainModel>(movieDetails);

            await this.PopulateFormMainModel(formModel);

            return View(formModel);
        }

        #endregion Edit HttpGet

        #region Edit HttpPost

        [HttpPost]
        public async Task<IActionResult> Edit(int id, MovieFormMainModel formModel, List<IFormFile> files)
        {
            bool isValid = await this.ValidateFormModelAndFiles(formModel, files);

            if (!isValid)
            {
                return View(formModel);
            }

            List<CastServiceModel> castArtists = this.mapper
                .Map<IEnumerable<CastFormModel>, List<CastServiceModel>>(formModel.Cast);

            List<CrewServiceModel> crewArtists = this.mapper
                .Map<IEnumerable<CrewFormModel>, List<CrewServiceModel>>(formModel.Crew);

            bool success = false;
            byte[] thumbnail = await this.GetThumbnail();

            try
            {
                success = await this.movieDbService.EditMovieAsync(
                    id,
                    formModel.BoxOfficeMojoId,
                    formModel.Budget,
                    castArtists.AsNotNull(),
                    crewArtists.AsNotNull(),
                    formModel.GrossForeign,
                    formModel.GrossUsa,
                    formModel.ImdbId,
                    formModel.ImdbTop250,
                    formModel.ImdbUsersRating,
                    formModel.OriginalTitle,
                    posters,
                    formModel.ProductionYear,
                    formModel.RottenTomatoesCriticsScore,
                    formModel.RottenTomatoesId,
                    formModel.RottenTomatoesUsersScore,
                    formModel.Runtime,
                    formModel.SelectedColors.AsNotNull(),
                    formModel.SelectedCountries.AsNotNull(),
                    formModel.SelectedGenres.AsNotNull(),
                    formModel.SelectedLanguages.AsNotNull(),
                    formModel.Synopsis,
                    thumbnail,
                    formModel.Title);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);

                await this.PopulateFormMainModel(formModel);

                return View(formModel);
            }

            if (success)
            {
                TempData[WebConstants.TempDataSuccessMessageKey]
                    = string.Format(MovieEditedSuccessMessage, formModel.Title);
            }
            else
            {
                TempData[WebConstants.TempDataErrorMessageKey]
                    = "Unsuccess";
            }

            return RedirectToAction(nameof(Index));
        }

        #endregion Edit HttpPost

        #region Search HttpGet

        public async Task<IActionResult> Search(string title)
        {
            if (title == null)
            {
                return BadRequest();
            }

            MovieSearchTitleFormModel formModel = new MovieSearchTitleFormModel();

            formModel.SearchTitleText = title;

            try
            {
                SearchTitleBaseServiceModel blurayDotComServiceModel
                        = await this.blurayDotComService.SearchTitleResultsAsync(title);

                formModel.BlurayDoComSearchTitleResults
                    = blurayDotComServiceModel.SearchTitleResults.AsNotNull();
            }
            catch (Exception ex)
            {
                formModel.BoxOfficeMojoExceptionMessage = ex.Message;
                await Task.Run(() => ex.Log(nameof(HomeController), nameof(Search)));
            }

            try
            {
                SearchTitleBaseServiceModel boxOfficeMojoServiceModel
                        = await this.boxOfficeMojoService.SearchTitleResultsAsync(title);

                formModel.BoxOfficeMojoSearchTitleResults
                    = boxOfficeMojoServiceModel.SearchTitleResults.AsNotNull();
            }
            catch (Exception ex)
            {
                formModel.BoxOfficeMojoExceptionMessage = ex.Message;
                await Task.Run(() => ex.Log(nameof(HomeController), nameof(Search)));
            }

            try
            {
                SearchTitleBaseServiceModel dvdEmpireServiceModel
                        = await this.dvdEmpireService.SearchTitleResultsAsync(title);

                formModel.DvdEmpireSearchTitleResults
                    = dvdEmpireServiceModel.SearchTitleResults.AsNotNull();
            }
            catch (Exception ex)
            {
                formModel.DvdEmpireExceptionMessage = ex.Message;
                await Task.Run(() => ex.Log(nameof(HomeController), nameof(Search)));
            }

            try
            {
                SearchTitleBaseServiceModel imdbServiceModel
                        = await this.imdbService.SearchTitleResultsAsync(title);

                formModel.ImdbSearchTitleResults
                    = imdbServiceModel.SearchTitleResults.AsNotNull();
            }
            catch (Exception ex)
            {
                formModel.ImdbExceptionMessage = ex.Message;
                await Task.Run(() => ex.Log(nameof(HomeController), nameof(Search)));
            }

            try
            {
                SearchTitleBaseServiceModel rottenTomatoesServiceModel
                        = await this.rottenTomatoesService.SearchTitleResultsAsync(title);

                formModel.RottenTomatoesSearchTitleResults
                    = rottenTomatoesServiceModel.SearchTitleResults.AsNotNull();
            }
            catch (Exception ex)
            {
                formModel.RottentTomattoesExceptionMessage = ex.Message;
                await Task.Run(() => ex.Log(nameof(HomeController), nameof(Search)));
            }

            return View(formModel);
        }

        #endregion Search HttpGet

        #region Search HttpPost

        [HttpPost]
        public async Task<IActionResult> Search(MovieSearchTitleFormModel formSearchModel)
        {
            const string none = WebConstants.RadioButtonNoneValue;

            string blurayDotComTitleId = formSearchModel.BlurayDotComSelectedTitle;
            string boxOfficeMojoTitleId = formSearchModel.BoxOfficeMojoSelectedTitle;
            string dvdEmpireTiteId = formSearchModel.DvdEmpireSelectedTitle;
            string imdbTitleId = formSearchModel.ImdbSelectedTitle;
            string rottenTomatoesTitleId = formSearchModel.RottenTomattoesSelectedTitle;

            MovieFormMainModel formMainModel = new MovieFormMainModel();

            #region Blu-ray.com

            if (!string.IsNullOrEmpty(blurayDotComTitleId) && blurayDotComTitleId != none)
            {
                //BlurayDotComMainServiceModel blurayDotComServiceModel
                //    = await this.blurayDotComService.GetMovieDataAsync(blurayDotComTitleId);
            }

            #endregion Blu-ray.com

            #region Box Office Mojo

            if (!string.IsNullOrEmpty(boxOfficeMojoTitleId) && boxOfficeMojoTitleId != none)
            {
                try
                {
                    BoxOfficeMojoMainServiceModel boxOfficeMojoServiceModel
                                = await this.boxOfficeMojoService.GetMovieDataAsync(boxOfficeMojoTitleId);

                    IMapper config = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<BoxOfficeMojoMainServiceModel, MovieFormMainModel>();
                    })
                    .CreateMapper();

                    config.Map(boxOfficeMojoServiceModel, formMainModel);
                }
                catch (Exception ex)
                {
                    await Task.Run(() => ex.Log(nameof(HomeController), nameof(Search)));
                }
            }

            #endregion Box Office Mojo

            #region IMDb

            if (!string.IsNullOrEmpty(imdbTitleId) && imdbTitleId != none)
            {
                try
                {
                    ImdbMainServiceModel imdbServiceModel
                                = await this.imdbService.GetMovieDataAsync(imdbTitleId);

                    this.mapper.Map(imdbServiceModel, formMainModel);

                    IEnumerable<string> colorNames = imdbServiceModel.Colors;
                    formMainModel.SelectedColors
                        = await this.movieDbService.GetColorsIdFromNameAsync(colorNames);

                    IEnumerable<string> countryNames = imdbServiceModel.Countries;
                    formMainModel.SelectedCountries
                        = await this.movieDbService.GetCountriesIdFromNameAsync(countryNames);

                    IEnumerable<string> genreNames = imdbServiceModel.Genres;
                    formMainModel.SelectedGenres
                        = await this.movieDbService.GetGenresIdFromNameAsync(genreNames);

                    IEnumerable<string> languageNames = imdbServiceModel.Languages;
                    formMainModel.SelectedLanguages
                        = await this.movieDbService.GetLanguagesIdFromNameAsync(languageNames);
                }
                catch (Exception ex)
                {
                    await Task.Run(() => ex.Log(nameof(HomeController), nameof(Search)));
                }
            }

            #endregion IMDb

            #region Rotten Tomattoes

            if (!string.IsNullOrEmpty(rottenTomatoesTitleId) && rottenTomatoesTitleId != none)
            {
                try
                {
                    RottenTomatoesMainServiceModel rottenTomatoesServiceModel
                                = await this.rottenTomatoesService.GetMovieDataAsync(rottenTomatoesTitleId);

                    IMapper config = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<RottenTomatoesMainServiceModel, MovieFormMainModel>();
                    })
                   .CreateMapper();

                    config.Map(rottenTomatoesServiceModel, formMainModel);
                }
                catch (Exception ex)
                {
                    await Task.Run(() => ex.Log(nameof(HomeController), nameof(Search)));
                }
            }

            #endregion Rotten Tomattoes

            await this.PopulateFormMainModel(formMainModel);

            return View("Edit", formMainModel);
        }

        #endregion Search HttpPost

        #region GetAllColors

        private async Task<IEnumerable<SelectListItem>> GetAllColors()
        {
            IEnumerable<ColorServiceModel> serviceModels
                = await this.movieDbService.GetAllColorsAsync();

            return serviceModels.Select(c => new SelectListItem()
            {
                Text = c.Name,
                Value = c.Id.ToString()
            });
        }

        #endregion GetAllColors

        #region GetAllCountries

        private async Task<IEnumerable<SelectListItem>> GetAllCountries()
        {
            IEnumerable<CountryServiceModel> serviceModels
                = await this.movieDbService.GetAllCountriesAsync();

            return serviceModels.Select(c => new SelectListItem()
            {
                Text = c.Name,
                Value = c.Id.ToString()
            });
        }

        #endregion GetAllCountries

        #region GetAllGenres

        private async Task<IEnumerable<SelectListItem>> GetAllGenres()
        {
            IEnumerable<GenreServiceModel> serviceModels
                = await this.movieDbService.GetAllGenresAsync();

            return serviceModels.Select(c => new SelectListItem()
            {
                Text = c.Name,
                Value = c.Id.ToString()
            });
        }

        #endregion GetAllGenres

        #region GetAllLanguages

        private async Task<IEnumerable<SelectListItem>> GetAllLanguages()
        {
            IEnumerable<LanguageServiceModel> serviceModels
                = await this.movieDbService.GetAllLanguagesAsync();

            return serviceModels.Select(c => new SelectListItem()
            {
                Text = c.Name,
                Value = c.Id.ToString()
            });
        }

        #endregion GetAllLanguages

        #region GetThumbnail

        private async Task<byte[]> GetThumbnail()
        {
            if (this.posters.Count() > 0)
            {
                byte[] mainPosterBytes = this.posters[0];
                Image mainPosterImage = await Task.Run(() => mainPosterBytes.ToImage());
                int thumbnailHeight = DataConstants.ThumbnailHeightPixels;
                int thumbnailWidth = (int)(thumbnailHeight / (mainPosterImage.Height / (double)mainPosterImage.Width));

                byte[] thumbnailBytes = await mainPosterBytes
                    .CreateThumbnail(thumbnailWidth, thumbnailHeight);

                Image thumbnailImage = await Task.Run(() => thumbnailBytes.ToImage());

                await Task.Run(() => thumbnailImage.Save(GlobalConstants.TempThumbnailFilePath));

                return thumbnailBytes;
            }

            return null;
        }

        #endregion GetThumbnail

        #region PopulateFormMainModel

        private async Task PopulateFormMainModel(MovieFormMainModel formModel)
        {
            formModel.AllColors = await this.GetAllColors();
            formModel.AllCountries = await this.GetAllCountries();
            formModel.AllGenres = await this.GetAllGenres();
            formModel.AllLanguages = await this.GetAllLanguages();
        }

        #endregion PopulateFormMainModel

        #region ValidateFormModelAndFiles

        private async Task<bool> ValidateFormModelAndFiles(MovieFormMainModel formModel, List<IFormFile> files)
        {
            if (!ModelState.IsValid)
            {
                await this.PopulateFormMainModel(formModel);

                return false;
            }

            this.posters = new List<byte[]>();
            foreach (IFormFile file in files)
            {
                bool isValidImage = await Task.Run(() => file.ValidateImage(ModelState));

                if (!isValidImage)
                {
                    await this.PopulateFormMainModel(formModel);

                    return false;
                }

                byte[] imageBytes = await file.ToByteArrayAsync();
                posters.Add(imageBytes);
            }

            return true;
        }

        #endregion ValidateFormModelAndFiles
    }
}
