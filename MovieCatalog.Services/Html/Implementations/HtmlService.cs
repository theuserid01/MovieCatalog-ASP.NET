namespace MovieCatalog.Services.Html.Implementations
{
    using Ganss.XSS;
    using MovieCatalog.Services.Html.Contracts;

    public class HtmlService : IHtmlService
    {
        private readonly HtmlSanitizer htmlSanitizer;

        public HtmlService()
        {
            this.htmlSanitizer = new HtmlSanitizer();
            this.htmlSanitizer.AllowedAttributes.Add("class");
        }

        public string Sanitize(string htmlContent)
        {
            return this.htmlSanitizer.Sanitize(htmlContent);
        }
    }
}
