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
    using MovieCatalog.Services.Html.Models.BoxOfficeMojo;

    public class BoxOfficeMojoService : BaseHtmlService, IBoxOfficeMojoService
    {
        #region GetMovieData

        public async Task<BoxOfficeMojoMainServiceModel> GetMovieDataAsync(string movieTitleId)
        {
            string contentFilePath = GlobalConstants.MainContentBoxOfficeMojoLogFilePath;
            string mainContentUrl = string.Format(GlobalConstants.BoxOfficeMojoMainContentUrl, movieTitleId);

            HtmlNode docNode = await GetDocNode(contentFilePath, mainContentUrl);

            HtmlNode budgetNode = docNode
                .SelectSingleNode("//td[contains(text(), 'Production Budget:')]/b[normalize-space(text())]");

            HtmlNode grossForeignNode = docNode
                .SelectSingleNode("//div[@class='mp_box_content']/*/tr[2]/td[2][normalize-space(text())]");

            HtmlNode grossUsaNode = docNode
                .SelectSingleNode("//div[@class='mp_box_content']/*/tr[1]/td[2][normalize-space(text())]");

            HtmlNode titleNode = docNode
                .SelectSingleNode("//font[@size='6']");

            decimal budget = await Task.Run(() => this.GetBudget(budgetNode));
            decimal grossForeign = await Task.Run(() => this.GetGross(grossForeignNode));
            decimal grossUsa = await Task.Run(() => this.GetGross(grossUsaNode));

            string title = string.Empty;
            if (titleNode != null)
            {
                title = titleNode.InnerText.Trim();
            }

            var model = new BoxOfficeMojoMainServiceModel()
            {
                BoxOfficeMojoId = movieTitleId,
                BoxOfficeMojoTitle = title,
                Budget = budget,
                GrossForeign = grossForeign,
                GrossUsa = grossUsa
            };

            return model;
        }

        #endregion GetMovieData

        #region SearchTitleResults

        public async Task<SearchTitleBaseServiceModel> SearchTitleResultsAsync(string movieTitle)
        {
            string contentFilePath = GlobalConstants.SearchResultsBoxOfficeMojoLogFilePath;
            string searchTitleUrl = string.Format(GlobalConstants.BoxOfficeMojoSearchTitleUrl, movieTitle.EncodeToValidUrl());

            HtmlNode docNode = await GetDocNode(contentFilePath, searchTitleUrl);

            var nodes = docNode
                .SelectNodes("//tr[.//a[starts-with(text(), 'Movie Title')]]/following-sibling::tr[count(td) > 2]");

            if (nodes == null)
            {
                throw new InvalidOperationException("Box Office Mojo search result failed!");
            }

            var model = new SearchTitleBaseServiceModel();

            foreach (HtmlNode tr in nodes)
            {
                HtmlNode td1 = tr.SelectSingleNode(".//td[1]");
                HtmlNode td2 = tr.SelectSingleNode(".//td[2]");
                string href = td1.Element("a").Attributes["href"].Value;
                string id = Regex.Match(href, @"id=(.*)$").Groups[1].Value;
                string title = td1.Element("b").InnerText.Trim();
                string studio = td2.InnerText.Trim();

                var kvp = new KeyValuePair<string, string>($"{title} ({studio})", id);

                model.SearchTitleResults.Add(kvp);
            }

            return model;
        }

        #endregion SearchTitleResults

        #region Budget

        private decimal GetBudget(HtmlNode budgetNode)
        {
            if (budgetNode != null)
            {
                string budgetText = budgetNode.InnerText.Trim();
                string budgetValue = Regex.Match(budgetText, @"\d+").Value;
                decimal budget = decimal.Parse(budgetValue) * 1_000_000;

                return budget;
            }

            return 0;
        }

        #endregion Budget

        #region Gross

        private decimal GetGross(HtmlNode node)
        {
            if (node != null)
            {
                return node
                    .InnerText.Trim()
                    .Replace("$", "")
                    .Replace(",", "")
                    .ParseToDecimal();
            }

            return 0;
        }

        #endregion Gross
    }
}
