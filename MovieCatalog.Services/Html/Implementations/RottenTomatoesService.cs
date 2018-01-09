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
    using MovieCatalog.Services.Html.Models.RottenTomatoes;
    using MovieCatalog.Services.Html.Models.RottenTomatoes.JsonDtos;
    using Newtonsoft.Json;

    public class RottenTomatoesService : BaseHtmlService, IRottenTomatoesService
    {
        #region GetMovieData

        public async Task<RottenTomatoesMainServiceModel> GetMovieDataAsync(string movieTitleId)
        {
            string contentFilePath = GlobalConstants.MainContentRottenTomatoesLogFilePath;
            string mainContentUrl = string.Format(GlobalConstants.RottenTomatoesMainContentUrl, movieTitleId);

            HtmlNode docNode = await GetDocNode(contentFilePath, mainContentUrl);

            HtmlNode criticsScoreNode = docNode
                .SelectSingleNode("//span[contains(@class, 'meter-value')]/span[normalize-space(text())]");

            HtmlNode titleNode = docNode
                .SelectSingleNode("//h1[@id='movie-title'][normalize-space(text())]");

            HtmlNode usersScoreNode = docNode
                .SelectSingleNode("//div[@class='meter-value']/span[normalize-space(text())]");

            if (criticsScoreNode == null)
            {
                throw new InvalidOperationException("Getting Rotten Tomatoes Critics Score failed!!");
            }

            if (usersScoreNode == null)
            {
                throw new InvalidOperationException("Getting Rotten Tomatoes Users Score failed!!");
            }

            string title = string.Empty;
            if (titleNode != null)
            {
                title = titleNode.InnerText.Trim();
            }

            string criticsScoreValue = criticsScoreNode.InnerText.Trim();
            int criticsScore = criticsScoreValue.ParseToInt();

            string usersScoreValue = usersScoreNode.InnerText.Replace("%", "").Trim();
            int usersScore = usersScoreValue.ParseToInt();

            var model = new RottenTomatoesMainServiceModel()
            {
                RottenTomatoesCriticsScore = criticsScore,
                RottenTomatoesId = movieTitleId,
                RottenTomatoesTitle = title,
                RottenTomatoesUsersScore = usersScore
            };

            return model;
        }

        #endregion GetMovieData

        #region SearchTitleResults

        public async Task<SearchTitleBaseServiceModel> SearchTitleResultsAsync(string movieTitle)
        {
            string contentFilePath = GlobalConstants.SearchResultsRottenTomatoesLogFilePath;
            string searchTitleUrl = string.Format(GlobalConstants.RottenTomatoesSearchTitleUrl, movieTitle.EncodeToValidUrl());

            HtmlNode docNode = await GetDocNode(contentFilePath, searchTitleUrl);

            Match match = Regex.Match(docNode.InnerHtml, @"(\{""actorCount"".*?""tvCount"":\d+\})");

            if (!match.Success)
            {
                throw new InvalidOperationException("Rotten Tomatoes search result failed!");
            }

            string json = match.Value;

            var searchTitleResults = JsonConvert
                .DeserializeObject<RottenTomatoesSearchMovieTitleJsonDtoModel>(json);

            var model = new SearchTitleBaseServiceModel();

            foreach (var movie in searchTitleResults.Movies)
            {
                string id = movie.Url.Trim().TrimStart('/');
                var kvp = new KeyValuePair<string, string>($"{movie.Name} ({movie.Year})", id);

                model.SearchTitleResults.Add(kvp);
            }

            return model;
        }

        #endregion SearchTitleResults
    }
}
