namespace MovieCatalog.Services.Html.Implementations
{
    using System.Threading.Tasks;
    using HtmlAgilityPack;
    using MovieCatalog.Common;

    public abstract class BaseHtmlService
    {
        protected async Task<HtmlNode> GetDocNode(string filePath, string url)
        {
            string htmlContent = await url.GetHtmlContentAsync();

            await htmlContent.WriteToFileAsync(filePath);

            HtmlDocument doc = new HtmlDocument();
            await Task.Run(() => doc.LoadHtml(htmlContent));

            return doc.DocumentNode;
        }
    }
}
