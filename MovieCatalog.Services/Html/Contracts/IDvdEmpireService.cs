namespace MovieCatalog.Services.Html.Contracts
{
    using System.Threading.Tasks;
    using MovieCatalog.Services.Html.Models;
    using MovieCatalog.Services.Html.Models.DvdEmpire;

    public interface IDvdEmpireService
    {
        Task<DvdEmpireMainServiceModel> GetMovieDataAsync(string movieTitleId);

        Task<SearchTitleBaseServiceModel> SearchTitleResultsAsync(string movieTitle);
    }
}
