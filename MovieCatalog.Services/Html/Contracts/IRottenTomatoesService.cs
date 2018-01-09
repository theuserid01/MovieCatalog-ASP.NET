namespace MovieCatalog.Services.Html.Contracts
{
    using System.Threading.Tasks;
    using MovieCatalog.Services.Html.Models;
    using MovieCatalog.Services.Html.Models.RottenTomatoes;

    public interface IRottenTomatoesService
    {
        Task<RottenTomatoesMainServiceModel> GetMovieDataAsync(string movieTitleId);

        Task<SearchTitleBaseServiceModel> SearchTitleResultsAsync(string movieTitle);
    }
}
