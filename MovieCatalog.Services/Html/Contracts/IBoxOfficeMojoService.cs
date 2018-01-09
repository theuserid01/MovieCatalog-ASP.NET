namespace MovieCatalog.Services.Html.Contracts
{
    using System.Threading.Tasks;
    using MovieCatalog.Services.Html.Models;
    using MovieCatalog.Services.Html.Models.BoxOfficeMojo;

    public interface IBoxOfficeMojoService
    {
        Task<BoxOfficeMojoMainServiceModel> GetMovieDataAsync(string movieTitleId);

        Task<SearchTitleBaseServiceModel> SearchTitleResultsAsync(string movieTitle);
    }
}
