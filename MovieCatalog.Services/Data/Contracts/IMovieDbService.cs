namespace MovieCatalog.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using MovieCatalog.Services.Data.Models.Movies;

    public interface IMovieDbService
    {
        Task<bool> CreateMovieAsync(
            string boxOfficeMojoId,
            decimal budget,
            IEnumerable<CastServiceModel> castArtists,
            IEnumerable<CrewServiceModel> crewwArtists,
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
            string title);

        Task<bool> DeleteMovieAsync(int movieId);

        Task<bool> EditMovieAsync(
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
            string title);

        Task<IEnumerable<ColorServiceModel>> GetAllColorsAsync();

        Task<IEnumerable<CountryServiceModel>> GetAllCountriesAsync();

        Task<IEnumerable<GenreServiceModel>> GetAllGenresAsync();

        Task<IEnumerable<LanguageServiceModel>> GetAllLanguagesAsync();

        Task<IEnumerable<MovieBaseServiceModel>> GetAllMoviesAsync();

        Task<IEnumerable<int>> GetColorsIdFromNameAsync(IEnumerable<string> colorNames);

        Task<IEnumerable<int>> GetCountriesIdFromNameAsync(IEnumerable<string> countryNames);

        Task<IEnumerable<int>> GetGenresIdFromNameAsync(IEnumerable<string> genreNames);

        Task<IEnumerable<int>> GetLanguagesIdFromNameAsync(IEnumerable<string> languageNames);

        Task<MovieDetailsServiceModel> GetMovieDetailsAsync(int movieId);
    }
}
