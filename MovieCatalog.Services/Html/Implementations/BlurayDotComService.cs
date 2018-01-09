namespace MovieCatalog.Services.Html.Implementations
{
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using HtmlAgilityPack;
    using MovieCatalog.Common;
    using MovieCatalog.Common.Extensions;
    using MovieCatalog.Services.Html.Contracts;
    using MovieCatalog.Services.Html.Models;
    using MovieCatalog.Services.Html.Models.BlurayDotCom;

    public class BlurayDotComService : BaseHtmlService, IBlurayDotComService
    {
        #region GetMovieData

        public async Task<BlurayDotComMainServiceModel> GetMovieDataAsync(string movieTitleId)
        {
            string contentFilePath = GlobalConstants.MainContentBlurayDotComLogFilePath;
            string mainContentUrl = string.Format(GlobalConstants.BlurayDotComMainContentUrl, movieTitleId);

            HtmlNode docNode = await GetDocNode(contentFilePath, mainContentUrl);

            var headingNode = docNode
                .SelectSingleNode("//span[@class='subheading'][contains(., '|')][a]");

            if (headingNode == null)
            {
                throw new InvalidOperationException("Getting Blu-ray.com Details failed!");
            }

            string title = string.Empty;
            HtmlNode titleNode = docNode.SelectSingleNode("//td[@width='518']");
            if (titleNode != null)
            {
                title = titleNode.InnerText.Trim();
            }

            string distributor = headingNode
                    .SelectSingleNode("./a[1]").InnerText.Trim();

            string productionYearValue = headingNode
                .SelectSingleNode("./a[2]").InnerText.Trim();
            int productionYear = productionYearValue.ParseToInt();

            string runtimeValue = headingNode
                .SelectSingleNode("./span[1]").InnerText.Trim()
                .Replace(" min", string.Empty);
            int runtime = runtimeValue.ParseToInt();

            string releaseDateValue = headingNode
                .SelectSingleNode("./a[3]").InnerText.Trim();
            DateTime releaseDate = releaseDateValue.ParseToDateTime();

            string html = docNode.InnerHtml;

            string contentRating = Regex.Match(html, @"\|\s+Rated\s+(.*?)\s+\|").Groups[1].Value;

            string codec = Regex
                .Match(html, @"Codec:\s+(.*?)(\s+\(.*?Mbps\))?<br>").Groups[1].Value;
            string resolution = Regex
                .Match(html, @"Resolution:\s+(.*?)\s*<br>").Groups[1].Value;
            string aspectRatio = Regex
                .Match(html, @"Aspect ratio:\s+(.*?)\s*<br>").Groups[1].Value;
            string originalAspectRatio = Regex
                .Match(html, @"Original aspect ratio:\s+(.*?)\s*<br>").Groups[1].Value;

            IEnumerable<string> audioTracks = null;
            HtmlNode audioNode = docNode
                .SelectSingleNode("//div[@id='longaudio']");
            if (audioNode != null)
            {
                HtmlNode a = audioNode.SelectSingleNode("./a");
                if (a != null)
                {
                    a.Remove();
                }

                audioTracks = audioNode.InnerHtml.Trim()
                    .Split(new[] { "<br>" }, StringSplitOptions.RemoveEmptyEntries);
            }

            IEnumerable<string> subtitles = null;
            HtmlNode subtitlesNode = docNode
                .SelectSingleNode("//div[@id='longsubs']/text()");
            if (subtitlesNode != null)
            {
                string subtitlesValue = subtitlesNode.InnerText.Trim();
                subtitles = subtitlesValue
                    .Split(new[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
            }

            var model = new BlurayDotComMainServiceModel()
            {
                AspectRatio = aspectRatio,
                // AudioTracks =
                BlurayDotComId = movieTitleId,
                ContentRating = contentRating,
                //DiscLayers =
                //DiscTotal =
                Distributor = distributor,
                OriginalAspectRatio = originalAspectRatio,
                ProductionYear = productionYear,
                ReleaseDate = releaseDate,
                Resolution = resolution,
                Runtime = runtime,
                Subtitles = subtitles,
                Title = title
            };

            return model;
        }

        #endregion GetMovieData

        #region SearchTitleResults

        public async Task<SearchTitleBaseServiceModel> SearchTitleResultsAsync(string movieTitle)
        {
            string contentFilePath = GlobalConstants.SearchResultsBlurayDotComLogFilePath;
            string searchTitleUrl = string.Format(GlobalConstants.BlurayDotComSearchTitleUrl, movieTitle.EncodeToValidUrl());

            HtmlNode docNode = await GetDocNode(contentFilePath, searchTitleUrl);

            var nodes = docNode
                .SelectNodes("//table[.//h2[contains(text(), 'Search movies')]]/following-sibling::table[.//a[@href and @title]]")
                .Descendants("a");

            if (nodes == null)
            {
                throw new InvalidOperationException("Blu-ray.com search result failed!");
            }

            var model = new SearchTitleBaseServiceModel();

            foreach (HtmlNode a in nodes)
            {
                string href = a.Attributes["href"].Value.Trim();
                string id = href
                    .Replace(GlobalConstants.BlurayDotComDomain + "/movies/", string.Empty)
                    .Trim('/');
                string title = a.Attributes["title"].Value.Trim();

                var kvp = new KeyValuePair<string, string>(title, id);

                model.SearchTitleResults.Add(kvp);
            }

            Console.WriteLine(model);

            return model;
        }

        #endregion SearchTitleResults
    }
}
