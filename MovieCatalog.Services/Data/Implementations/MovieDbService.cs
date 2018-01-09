namespace MovieCatalog.Services.Data.Implementations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper.QueryableExtensions;
    using Microsoft.EntityFrameworkCore;
    using MovieCatalog.Data;
    using MovieCatalog.Data.Models;
    using MovieCatalog.Data.Models.ManyToMany;
    using MovieCatalog.Services.Data.Contracts;
    using MovieCatalog.Services.Data.Models.Movies;

    public class MovieDbService : IMovieDbService
    {
        private const string IdNotExistErrorMessage = "{0} with Id {1} doesn't exist!";

        private MovieCatalogDbContext context;

        public MovieDbService(MovieCatalogDbContext context)
        {
            this.context = context;
        }

        #region CreateMovie

        public async Task<bool> CreateMovieAsync(
            string boxOfficeMojoId,
            decimal budget,
            IEnumerable<CastServiceModel> castArtists,
            IEnumerable<CrewServiceModel> crewArtists,
            decimal grossForeign,
            decimal grossUsa,
            string imdbId,
            int imdbTop250,
            double imdbUsersRating,
            string originalTitle,
            IEnumerable<byte[]> posters,
            int productionYear,
            int rottenTomatoesCriticsScore,
            string rottenTomatoesId,
            int rottenTomatoesUsersScore,
            int runtime,
            IEnumerable<int> selectedColors,
            IEnumerable<int> selectedCountries,
            IEnumerable<int> selectedGenres,
            IEnumerable<int> selectedLanguages,
            string synopsis,
            byte[] thumbnail,
            string title)
        {
            Movie movie = new Movie()
            {
                BoxOfficeMojoId = boxOfficeMojoId,
                Budget = budget,
                CreationDate = DateTime.UtcNow,
                GrossForeign = grossForeign,
                GrossUsa = grossUsa,
                ImdbId = imdbId,
                ImdbTop250 = imdbTop250,
                ImdbUsersRating = imdbUsersRating,
                OriginalTitle = originalTitle,
                ProductionYear = productionYear,
                RottenTomatoesCriticsScore = rottenTomatoesCriticsScore,
                RottenTomatoesId = rottenTomatoesId,
                RottenTomatoesUsersScore = rottenTomatoesUsersScore,
                Runtime = runtime,
                Synopsis = synopsis,
                Thumbnail = thumbnail,
                Title = title
            };

            this.context.Add(movie);

            foreach (byte[] poster in posters)
            {
                movie.Posters.Add(new Poster() { Image = poster });
            }

            await this.SaveMovieCastAsync(movie, castArtists);

            await this.SaveMovieCrewAsync(movie, crewArtists);

            await this.SaveMovieColorsAsync(movie, selectedColors);

            await this.SaveMovieCountriesAsync(movie, selectedCountries);

            await this.SaveMovieGenresAsync(movie, selectedGenres);

            await this.SaveMovieLanguagesAsync(movie, selectedLanguages);

            await this.context.SaveChangesAsync();

            return true;
        }

        #endregion CreateMovie

        #region DeleteMovie

        public async Task<bool> DeleteMovieAsync(int movieId)
        {
            Movie movie = await this.context.Movies
                .FirstOrDefaultAsync(m => m.Id == movieId);

            if (movie == null)
            {
                return false;
            }

            this.context.Remove(movie);

            await this.context.SaveChangesAsync();

            return true;
        }

        #endregion DeleteMovie

        #region EditMovie

        public async Task<bool> EditMovieAsync(
            int movieId,
            string boxOfficeMojoId,
            decimal budget,
            IEnumerable<CastServiceModel> castArtists,
            IEnumerable<CrewServiceModel> crewArtists,
            decimal grossForeign,
            decimal grossUsa,
            string imdbId,
            int imdbTop250,
            double imdbUsersRating,
            string originalTitle,
            IEnumerable<byte[]> posters,
            int productionYear,
            int rottenTomatoesCriticsScore,
            string rottenTomatoesId,
            int rottenTomatoesUsersScore,
            int runtime,
            IEnumerable<int> selectedColors,
            IEnumerable<int> selectedCountries,
            IEnumerable<int> selectedGenres,
            IEnumerable<int> selectedLanguages,
            string synopsis,
            byte[] thumbnail,
            string title)
        {
            Movie movie = await this.context.Movies
                .FirstOrDefaultAsync(m => m.Id == movieId);

            if (movie == null)
            {
                return false;
            }

            movie.BoxOfficeMojoId = boxOfficeMojoId;
            movie.Budget = budget;
            movie.GrossForeign = grossForeign;
            movie.GrossUsa = grossUsa;
            movie.ImdbId = imdbId;
            movie.ImdbTop250 = imdbTop250;
            movie.ImdbUsersRating = imdbUsersRating;
            movie.OriginalTitle = originalTitle;
            movie.ProductionYear = productionYear;
            movie.RottenTomatoesCriticsScore = rottenTomatoesCriticsScore;
            movie.RottenTomatoesId = movie.RottenTomatoesId;
            movie.RottenTomatoesUsersScore = rottenTomatoesUsersScore;
            movie.Runtime = runtime;
            movie.Synopsis = synopsis;
            movie.Thumbnail = thumbnail ?? movie.Thumbnail;
            movie.Title = title;

            IEnumerable<MovieCast> existingMovieCasts = this.context.MoviesCasts
                .Where(mc => mc.MovieId == movie.Id);

            IEnumerable<MovieCrew> existingMovieCrews = this.context.MoviesCrews
                .Where(mc => mc.MovieId == movie.Id);

            this.context.RemoveRange(existingMovieCasts);

            this.context.RemoveRange(existingMovieCrews);

            await this.context.SaveChangesAsync();

            var existingMoviePosters = movie.Posters
                .Select(p => p.Image);
            foreach (byte[] poster in posters)
            {
                if (existingMoviePosters.Any(p => p.Length == poster.Length))
                {
                    continue;
                }

                movie.Posters.Add(new Poster() { Image = poster });
            }

            await this.SaveMovieCastAsync(movie, castArtists);

            await this.SaveMovieColorsAsync(movie, selectedColors);

            await this.SaveMovieCountriesAsync(movie, selectedCountries);

            await this.SaveMovieCrewAsync(movie, crewArtists);

            await this.SaveMovieGenresAsync(movie, selectedGenres);

            await this.SaveMovieLanguagesAsync(movie, selectedLanguages);

            await this.context.SaveChangesAsync();

            this.context.Update(movie);

            await this.context.SaveChangesAsync();

            return true;
        }

        #endregion EditMovie

        #region GetAllColors

        public async Task<IEnumerable<ColorServiceModel>> GetAllColorsAsync()
        {
            return await this.context.Colors
                .ProjectTo<ColorServiceModel>()
                .ToListAsync();
        }

        #endregion GetAllColors

        #region GetAllCountries

        public async Task<IEnumerable<CountryServiceModel>> GetAllCountriesAsync()
        {
            return await this.context.Countries
                .ProjectTo<CountryServiceModel>()
                .ToListAsync();
        }

        #endregion GetAllCountries

        #region GetAllGenres

        public async Task<IEnumerable<GenreServiceModel>> GetAllGenresAsync()
        {
            return await this.context.Genres
                .ProjectTo<GenreServiceModel>()
                .ToListAsync();
        }

        #endregion GetAllGenres

        #region GetAllLanguages

        public async Task<IEnumerable<LanguageServiceModel>> GetAllLanguagesAsync()
        {
            return await this.context.Languages
                .ProjectTo<LanguageServiceModel>()
                .ToListAsync();
        }

        #endregion GetAllLanguages

        #region GetAllMovies

        public async Task<IEnumerable<MovieBaseServiceModel>> GetAllMoviesAsync()
        {
            return await this.context.Movies
                .ProjectTo<MovieBaseServiceModel>()
                .OrderByDescending(m => m.Id)
                .ToListAsync();
        }

        #endregion GetAllMovies

        #region GetColorsIdFromName

        public async Task<IEnumerable<int>> GetColorsIdFromNameAsync(IEnumerable<string> colorNames)
        {
            return await this.context.Colors
                .Where(c => colorNames.Contains(c.Name))
                .Select(c => c.Id)
                .ToListAsync();
        }

        #endregion GetColorsIdFromName

        #region GetCountriesIdFromName

        public async Task<IEnumerable<int>> GetCountriesIdFromNameAsync(IEnumerable<string> countryNames)
        {
            return await this.context.Countries
                .Where(c => countryNames.Contains(c.Name))
                .Select(c => c.Id)
                .ToListAsync();
        }

        #endregion GetCountriesIdFromName

        #region GetGenresIdFromName

        public async Task<IEnumerable<int>> GetGenresIdFromNameAsync(IEnumerable<string> genreNames)
        {
            return await this.context.Genres
                .Where(g => genreNames.Contains(g.Name))
                .Select(g => g.Id)
                .ToListAsync();
        }

        #endregion GetGenresIdFromName

        #region GetLanguagesIdFromName

        public async Task<IEnumerable<int>> GetLanguagesIdFromNameAsync(IEnumerable<string> languageNames)
        {
            return await this.context.Languages
                .Where(l => languageNames.Contains(l.Name))
                .Select(l => l.Id)
                .ToListAsync();
        }

        #endregion GetLanguagesIdFromName

        #region GetMovieDetails

        public async Task<MovieDetailsServiceModel> GetMovieDetailsAsync(int movieId)
        {
            MovieDetailsServiceModel movie = await this.context.Movies
                .Where(m => m.Id == movieId)
                .ProjectTo<MovieDetailsServiceModel>()
                .FirstOrDefaultAsync();

            return movie;
        }

        #endregion GetMovieDetails

        #region GetOrSaveArtist

        private async Task<int> GetOrSaveArtistAsync(string imdbId, string name, string photoUrl)
        {
            if (imdbId == null)
            {
                throw new ArgumentException($"Artist {name} IMDb Id cannot be null!");
            }

            Artist existingArtist = await this.context.Artists
                .FirstOrDefaultAsync(a => a.ImdbId == imdbId);

            if (existingArtist != null)
            {
                return existingArtist.Id;
            }

            Artist newArtist = new Artist()
            {
                ImdbId = imdbId,
                Name = name
            };

            await this.context.AddAsync(newArtist);

            await this.context.SaveChangesAsync();

            return newArtist.Id;
        }

        #endregion GetOrSaveArtist

        #region SaveMovieCast

        private async Task SaveMovieCastAsync(Movie movie, IEnumerable<CastServiceModel> castArtists)
        {
            foreach (var artist in castArtists)
            {
                int artistId = await this.GetOrSaveArtistAsync(artist.ImdbId, artist.Name, artist.PhotoUrl);

                await this.context.AddAsync(new MovieCast()
                {
                    ArtistId = artistId,
                    MovieId = movie.Id,
                    Character = artist.Character
                });
            }

            await this.context.SaveChangesAsync();
        }

        #endregion SaveMovieCast

        #region SaveMovieCrew

        private async Task SaveMovieCrewAsync(Movie movie, IEnumerable<CrewServiceModel> crewArtists)
        {
            foreach (var artist in crewArtists)
            {
                int artistId = await this.GetOrSaveArtistAsync(artist.ImdbId, artist.Name, artist.PhotoUrl);

                await this.context.AddAsync(new MovieCrew()
                {
                    ArtistId = artistId,
                    MovieId = movie.Id,
                    Role = artist.Role
                });
            }

            await this.context.SaveChangesAsync();
        }

        #endregion SaveMovieCrew

        #region SaveMovieColors

        private async Task SaveMovieColorsAsync(Movie movie, IEnumerable<int> selectedColors)
        {
            foreach (int colorId in selectedColors)
            {
                Color color = await this.context.Colors
                    .FirstOrDefaultAsync(c => c.Id == colorId);

                if (color == null)
                {
                    throw new ArgumentException(string.Format(IdNotExistErrorMessage, "Color", colorId));
                }

                bool movieColorExists = await this.context.MoviesColors
                    .AnyAsync(mc => mc.MovieId == movie.Id && mc.ColorId == color.Id);

                if (movieColorExists)
                {
                    continue;
                }

                await this.context.MoviesColors
                    .AddAsync(new MovieColor() { MovieId = movie.Id, ColorId = color.Id });
            }
        }

        #endregion SaveMovieColors

        #region SaveMovieCountries

        private async Task SaveMovieCountriesAsync(Movie movie, IEnumerable<int> selectedCountries)
        {
            foreach (int countryId in selectedCountries)
            {
                Country country = await this.context.Countries
                    .FirstOrDefaultAsync(c => c.Id == countryId);

                if (country == null)
                {
                    throw new ArgumentException(string.Format(IdNotExistErrorMessage, "Country", countryId));
                }

                bool movieCountryExists = await this.context.MoviesCountries
                    .AnyAsync(mc => mc.MovieId == movie.Id && mc.CountryId == country.Id);

                if (movieCountryExists)
                {
                    continue;
                }

                await this.context.MoviesCountries
                    .AddAsync(new MovieCountry() { MovieId = movie.Id, CountryId = country.Id });
            }
        }

        #endregion SaveMovieCountries

        #region SaveMovieGenres

        private async Task SaveMovieGenresAsync(Movie movie, IEnumerable<int> selectedGenres)
        {
            foreach (int genreId in selectedGenres)
            {
                Genre genre = await this.context.Genres
                    .FirstOrDefaultAsync(g => g.Id == genreId);

                if (genre == null)
                {
                    throw new ArgumentException(string.Format(IdNotExistErrorMessage, "Genre", genreId));
                }

                bool movieGenreExists = await this.context.MoviesGenres
                    .AnyAsync(mg => mg.MovieId == movie.Id && mg.GenreId == genreId);

                if (movieGenreExists)
                {
                    continue;
                }

                await this.context.MoviesGenres
                    .AddAsync(new MovieGenre() { MovieId = movie.Id, GenreId = genre.Id });
            }
        }

        #endregion SaveMovieGenres

        #region SaveMovieLanguages

        private async Task SaveMovieLanguagesAsync(Movie movie, IEnumerable<int> selectedLanguages)
        {
            foreach (int languageId in selectedLanguages)
            {
                Language language = await this.context.Languages
                    .FirstOrDefaultAsync(l => l.Id == languageId);

                if (language == null)
                {
                    throw new ArgumentException(string.Format(IdNotExistErrorMessage, "Language", languageId));
                }

                bool movieLanguageExists = await this.context.MoviesLanguages
                    .AnyAsync(ml => ml.MovieId == movie.Id && ml.LanguageId == language.Id);

                if (movieLanguageExists)
                {
                    continue;
                }

                await this.context
                    .AddAsync(new MovieLanguage() { MovieId = movie.Id, LanguageId = languageId });
            }
        }

        #endregion SaveMovieLanguages
    }
}
