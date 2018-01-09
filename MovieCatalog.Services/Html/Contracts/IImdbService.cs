namespace MovieCatalog.Services.Html.Contracts
{
    using System.Threading.Tasks;
    using MovieCatalog.Services.Html.Models;
    using MovieCatalog.Services.Html.Models.Imdb;

    public interface IImdbService
    {
        Task<ImdbMainServiceModel> GetMovieDataAsync(string movieTitleId);

        Task<SearchTitleBaseServiceModel> SearchTitleResultsAsync(string movieTitle);
    }
}
