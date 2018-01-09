namespace MovieCatalog.Services.Html.Contracts
{
    using System.Threading.Tasks;
    using MovieCatalog.Services.Html.Models;
    using MovieCatalog.Services.Html.Models.BlurayDotCom;

    public interface IBlurayDotComService
    {
        Task<BlurayDotComMainServiceModel> GetMovieDataAsync(string movieTitleId);

        Task<SearchTitleBaseServiceModel> SearchTitleResultsAsync(string movieTitle);
    }
}
