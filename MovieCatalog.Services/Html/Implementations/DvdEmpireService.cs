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
    using MovieCatalog.Services.Html.Models.DvdEmpire;

    public class DvdEmpireService : BaseHtmlService, IDvdEmpireService
    {
        #region GetMovieData

        public async Task<DvdEmpireMainServiceModel> GetMovieDataAsync(string movieTitleId)
        {
            string contentFilePath = GlobalConstants.MainContentDvdEmpireLogFilePath;
            string mainContentUrl = string.Format(GlobalConstants.DvdEmpireMainContentUrl, movieTitleId);

            HtmlNode docNode = await GetDocNode(contentFilePath, mainContentUrl);

            HtmlNode titleNode = docNode
                .SelectSingleNode("//div[@class='container text-center']/h2[normalize-space(text())]");

            HtmlNode synopsisNode = docNode
                .SelectSingleNode("//div[@class='container text-center']/h4/p[normalize-space(text())]");

            HtmlNode detailsNode = docNode
                .SelectSingleNode("//div[h2[contains(text(), 'Details')]]/ul[li]");

            if (titleNode == null)
            {
                throw new InvalidOperationException("Getting DVD Empire Movie Title failed!");
            }

            if (synopsisNode == null)
            {
                throw new InvalidOperationException("Getting DVD Empire Synopsis failed!");
            }

            if (detailsNode == null)
            {
                throw new InvalidOperationException("Getting DVD Empire Details failed!");
            }

            string title = titleNode.InnerText.Trim();

            string synopsis = synopsisNode.InnerHtml.Trim();

            int runtime = 0;
            HtmlNode runtimeNode = detailsNode
                .SelectSingleNode("./li[strong[contains(text(), 'Length')]]/time[normalize-space(text())]");
            if (runtimeNode != null)
            {
                string runtimeValue = runtimeNode.InnerText.Trim();

                int hrs = Regex.Match(runtimeValue, @"(\d+)\s+hrs")
                    .Groups[1].Value
                    .ParseToInt();

                int min = Regex.Match(runtimeValue, @"(\d+)\s+min")
                    .Groups[1].Value
                    .ParseToInt();

                runtime = hrs * 60 + min;
            }

            string contentRating = null;
            HtmlNode contentRatingNode = detailsNode
                .SelectSingleNode("./li[strong[contains(text(), 'Rating')]]/span[normalize-space(text())]");
            if (contentRatingNode != null)
            {
                contentRating = contentRatingNode.InnerText.Trim();
            }

            DateTime releaseDate = new DateTime(1900, 1, 1);
            HtmlNode releaseDateNode = detailsNode
                .SelectSingleNode("./li[strong[contains(text(), 'Released')]]/time[normalize-space(text())]");
            if (releaseDateNode != null)
            {
                string releaseDateValue = releaseDateNode.InnerText.Trim();
                releaseDate = releaseDateValue.ParseToDateTime();
            }

            int productionYear = 0;
            HtmlNode productionYearNode = detailsNode
                .SelectSingleNode("./li[strong[contains(text(), 'Production Year')]]/text()");
            if (productionYearNode != null)
            {
                string productionYearValue = productionYearNode.InnerText.Trim();
                productionYear = productionYearValue.ParseToInt();
            }

            string barcode = null;
            HtmlNode barcodeNode = detailsNode
                .SelectSingleNode("./li[strong[contains(text(), 'UPC Code')]]/text()");
            if (barcodeNode != null)
            {
                barcode = barcodeNode.InnerText.Trim();
            }

            string distributor = null;
            HtmlNode distributorNode = detailsNode
                .SelectSingleNode("./li[strong[contains(text(), 'Studio')]]/a[normalize-space(text())]");
            if (distributorNode != null)
            {
                distributor = distributorNode.InnerText.Trim();
            }

            int discTotal = 0;
            HtmlNode discTotalNode = detailsNode
                .SelectSingleNode("./li[strong[contains(text(), 'Number of Discs')]]/text()");
            if (discTotalNode != null)
            {
                string discTotalValue = discTotalNode.InnerText.Trim();
                discTotal = discTotalValue.ParseToInt();
            }

            string discLayers = null;
            HtmlNode discLayersNode = detailsNode
                .SelectSingleNode("./li[strong[starts-with(text(), 'Disc')]]/text()");
            if (discLayersNode != null)
            {
                discLayers = discLayersNode.InnerText.Trim();
            }

            IEnumerable<string> subtitles = null;
            HtmlNode subtitlesNode = detailsNode
                .SelectSingleNode("./li[strong[contains(text(), 'Subtitles')]]/text()");
            if (subtitlesNode != null)
            {
                string subtitlesValue = subtitlesNode
                    .InnerText.Trim()
                    .Replace(" and", ",");

                subtitles = subtitlesValue
                    .Split(new[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
            }

            string specialFeatures = null;
            HtmlNode specialFeaturesNode = docNode
                .SelectSingleNode("//div[h2[contains(text(), 'Features')]]/p[normalize-space(text())]");
            if (specialFeaturesNode != null)
            {
                specialFeatures = specialFeaturesNode.InnerHtml.Trim();
            }

            var model = new DvdEmpireMainServiceModel()
            {
                Barcode = barcode,
                ContentRating = contentRating,
                DiscLayers = discLayers,
                DiscTotal = discTotal,
                Distributor = distributor,
                DvdEmpireId = movieTitleId,
                ProductionYear = productionYear,
                ReleaseDate = releaseDate,
                Runtime = runtime,
                Specialfeatures = specialFeatures,
                Subtitles = subtitles,
                Synopsis = synopsis,
                Title = title
            };

            return model;
        }

        #endregion GetMovieData

        #region SearchTitleResults

        public async Task<SearchTitleBaseServiceModel> SearchTitleResultsAsync(string movieTitle)
        {
            string contentFilePath = GlobalConstants.SearchResultsDvdEmpireLogIlePath;
            string searchTitleUrl = string.Format(GlobalConstants.DvdEmpireSearchTileUrl, movieTitle.EncodeToValidUrl());

            HtmlNode docNode = await GetDocNode(contentFilePath, searchTitleUrl);

            // Search for <h3> with childnodes <a> AND <small> AND sibling <ul>
            var nodes = docNode
                .SelectNodes("//div[@id='ListView']/div/*/h3[a and small][../ul]");

            if (nodes == null)
            {
                throw new InvalidOperationException("DVD Empire search result failed!");
            }

            var model = new SearchTitleBaseServiceModel();

            foreach (HtmlNode h3 in nodes)
            {
                string href = h3.Element("a").Attributes["href"].Value;
                string id = href.Trim().TrimStart('/');
                string title = h3.Element("a").InnerText.Trim();
                HtmlNode small = h3.SelectSingleNode("./small[2]/text()");
                HtmlNode span = h3.SelectSingleNode("../ul/li[3]/span/text()");

                string productionYear = string.Empty;
                if (small != null)
                {
                    productionYear = $"{small.InnerText.Trim()}";
                    title = $"{title} {productionYear}";
                }

                string releaseDateValue = string.Empty;
                if (span != null)
                {
                    releaseDateValue = span.InnerText.Trim();
                    DateTime releaseDate = releaseDateValue.ParseToDateTime();
                    title = $"{title} (Home Video Release Date: {releaseDate.ToShortDateString()})";
                }

                var kvp = new KeyValuePair<string, string>($"{title}", id);

                model.SearchTitleResults.Add(kvp);
            }

            return model;
        }

        #endregion SearchTitleResults
    }
}
