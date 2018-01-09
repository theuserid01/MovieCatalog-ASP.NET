namespace MovieCatalog.Services.Html.Contracts
{
    public interface IHtmlService
    {
        string Sanitize(string htmlContent);
    }
}
