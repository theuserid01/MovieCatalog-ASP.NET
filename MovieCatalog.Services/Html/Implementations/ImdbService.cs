namespace MovieCatalog.Services.Html.Implementations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using HtmlAgilityPack;
    using MovieCatalog.Common;
    using MovieCatalog.Common.Extensions;
    using MovieCatalog.Data.Models.Enums;
    using MovieCatalog.Services.Extensions;
    using MovieCatalog.Services.Html.Contracts;
    using MovieCatalog.Services.Html.Models;
    using MovieCatalog.Services.Html.Models.Imdb;

    public class ImdbService : BaseHtmlService, IImdbService
    {
        private const string DateAddedValue = "1 {0}";

        private HtmlNode docMain;
        private IDictionary<string, IList<ImdbAwardServiceModel>> awards;

        #region GetMovieData

        public async Task<ImdbMainServiceModel> GetMovieDataAsync(string movieTitleId)
        {
            string contentFilePath = GlobalConstants.MainContentImdbLogFilePath;
            string mainContentUrl = string.Format(GlobalConstants.ImdbMainContentUrl, movieTitleId);

            this.docMain = await GetDocNode(contentFilePath, mainContentUrl);

            (int productionYear, string title) = await Task.Run(() => this.GetProductionYearAndTitle());

            var model = new ImdbMainServiceModel
            {
                //Awards = await Task.Run(() => this.GetAwards(movieTitleId)),
                Cast = await Task.Run(() => this.GetCast()),
                Colors = await Task.Run(() => this.GetZebraListValues("Color")),
                Countries = await Task.Run(() => this.GetZebraListValues("Country")),
                Crew = await Task.Run(() => this.GetCrew()),
                Genres = await Task.Run(() => this.GetZebraListValues("Genre")),
                ImdbId = movieTitleId,
                ImdbTop250 = await Task.Run(() => this.GetTop250()),
                ImdbUsersRating = await Task.Run(() => this.GetUsersRating()),
                Languages = await Task.Run(() => this.GetZebraListValues("Language")),
                OriginalTitle = await Task.Run(() => this.GetOriginalTitle()),
                ProductionYear = productionYear,
                //Releases = await Task.Run(() => this.GetReleaseInfo(movieTitleId)),
                Runtime = await Task.Run(() => this.GetRuntime("Runtime")),
                Studios = await Task.Run(() => this.GetStudios()),
                Synopsis = await Task.Run(() => this.GetSynopsis()),
                Title = title
            };

            //this.DownloadPoster();

            return model;
        }

        #endregion GetMovieData

        #region SearchTitleResults

        public async Task<SearchTitleBaseServiceModel> SearchTitleResultsAsync(string movieTitle)
        {
            string contentFilePath = GlobalConstants.SearchResultsImdbLogFilePath;
            string searchTitleUrl = string.Format(GlobalConstants.ImdbSearchTitleUrl, movieTitle.EncodeToValidUrl());

            HtmlNode docNode = await GetDocNode(contentFilePath, searchTitleUrl);

            var nodes = docNode
                .SelectNodes("//div[@class='findSection']/table[1]/tr");

            if (nodes == null)
            {
                throw new InvalidOperationException("IMDb search result failed!");
            }

            var model = new SearchTitleBaseServiceModel();

            foreach (HtmlNode tr in nodes)
            {
                if (!tr.Descendants("td").Any() || tr.Descendants("td").Count() < 2)
                {
                    continue;
                }

                HtmlNode td2 = tr.SelectSingleNode(".//td[2]");
                string title = td2.InnerText.Trim();
                title = title.RemoveExtraWhitespace();
                string href = td2.Element("a").GetAttributeValue("href", string.Empty);
                string id = Regex.Match(href, @"(tt\d+)").Value;

                var kvp = new KeyValuePair<string, string>(title, id);

                model.SearchTitleResults.Add(kvp);
            }

            return model;
        }

        #endregion SearchTitleResults

        #region Awards

        private async Task<IDictionary<string, IList<ImdbAwardServiceModel>>> GetAwards(string movieTitleId)
        {
            string contentFilePath = GlobalConstants.ImdbAwardsLogFilePath;
            string awardsUrl = string.Format(GlobalConstants.ImdbAwardsUrl, movieTitleId);

            HtmlNode docAwards = await GetDocNode(contentFilePath, awardsUrl);

            var awardTypes = new Dictionary<string, string>()
            {
                { "Academy Awards, USA", "Oscar" },
                { "Golden Globes, USA", "Golden Globe" }
            };

            this.awards = new Dictionary<string, IList<ImdbAwardServiceModel>>();
            foreach (var kvp in awardTypes)
            {
                string awardType = kvp.Value;

                this.awards[awardType] = new List<ImdbAwardServiceModel>();

                var nodes = docAwards.SelectNodes("//h3")
                    .Where(tag => new Regex(kvp.Key).IsMatch(tag.InnerText));

                foreach (HtmlNode h3 in nodes)
                {
                    int awardYear = int.Parse(h3.Element("a").InnerText.Trim());

                    var wonNodes = h3.SelectSingleNode("following-sibling::table")
                        .Descendants("tr")
                        .TakeWhile(tag => !tag.InnerText.Contains("Nominated"));

                    var nomNodes = h3.SelectSingleNode("following-sibling::table")
                        .Descendants("tr")
                        .SkipWhile(tag => !tag.InnerText.Contains("Nominated"));

                    if (wonNodes.Any())
                    {
                        this.GetAwardCategories(wonNodes, AwardRole.Won, awardType, awardYear);
                    }

                    if (nomNodes.Any())
                    {
                        this.GetAwardCategories(nomNodes, AwardRole.Nominated, awardType, awardYear);
                    }
                }
            }

            return this.awards;
        }

        private void GetAwardCategories(IEnumerable<HtmlNode> categoryNodes, AwardRole awardRole, string awardType, int awardYear)
        {
            foreach (HtmlNode categoryNode in categoryNodes)
            {
                string category = categoryNode
                    .SelectSingleNode(".//td[@class='award_description']/text()")
                    .InnerText.Trim();

                string notes = null;
                var notesNodes = categoryNode
                    .SelectNodes(".//td[@class='award_description']")
                    .Where(tag => tag.Descendants("p").Any());

                if (notesNodes.Any())
                {
                    notes = notesNodes
                        .FirstOrDefault()
                        .Descendants("p")
                        .Where(tag => tag.Attributes["class"].Value.Contains("full-note"))
                        .FirstOrDefault()
                        .InnerText.Trim();
                }

                IEnumerable<string> recipients = categoryNode
                    .SelectNodes(".//td[@class='award_description']")
                    .Descendants("a")
                    .Where(tag => tag.ParentNode.Name != "p")
                    .Select(tag => tag.InnerText.Trim());

                this.awards[awardType].Add(new ImdbAwardServiceModel()
                {
                    Category = category,
                    Notes = notes,
                    Recipients = recipients,
                    Role = awardRole,
                    Year = awardYear
                });
            }
        }

        #endregion Awards

        #region Cast

        private IEnumerable<ImdbCastServiceModel> GetCast()
        {
            var nodes = this.docMain
                .SelectNodes("//table[@class='cast_list']//tr[@class!='']");

            if (nodes == null)
            {
                throw new InvalidOperationException("IMDb collecting Cast table failed!");
            }

            var cast = new List<ImdbCastServiceModel>();
            foreach (HtmlNode tr in nodes.Take(GlobalConstants.ImdbCastNumber))
            {
                HtmlNode ch = tr.SelectSingleNode(".//td[@class='character']");
                HtmlNode nm = tr.SelectSingleNode(".//td[@class='itemprop']");

                if (ch == null || nm == null)
                {
                    throw new InvalidOperationException("IMDb collecting Cast row failed!");
                }

                string href = nm.Element("a").Attributes["href"].Value;
                string id = Regex.Match(href, @"(nm\d+)").Value;
                string name = nm.InnerText.Trim().RemoveExtraWhitespace();
                string character = ch.InnerText.Trim().RemoveExtraWhitespace();

                string photoLink = tr.Descendants("img")
                    .FirstOrDefault()
                    .Attributes["src"].Value;

                string photoUrl = null;
                if (!photoLink.ToLower().Contains("no_photo"))
                {
                    photoUrl = Regex.Replace(photoLink, @"\d+_", "5000_");
                }

                cast.Add(new ImdbCastServiceModel()
                {
                    Character = character,
                    ImdbId = id,
                    Name = name,
                    PhotoUrl = photoUrl
                });
            }

            return cast;
        }

        #endregion Cast

        #region Crew

        private IEnumerable<ImdbCrewServiceModel> GetCrew()
        {
            string[] crewRoles = Enum.GetNames(typeof(CrewRole));
            Regex patternSkip = new Regex(@"assistant|associate|co-|coordinating|executive|line|supervising|supervisor|uncredited");

            var crew = new List<ImdbCrewServiceModel>();
            foreach (string role in crewRoles)
            {
                var nodes = this.docMain
                    .SelectNodes($"//header[.//h4[starts-with(@name, '{role.ToLower()}')]]/following-sibling::table[1][.//a[normalize-space(text())]]");

                if (nodes == null)
                {
                    // TODO Log
                    continue;
                }

                var dupes = new HashSet<string>();
                foreach (HtmlNode tr in nodes.Descendants("tr"))
                {
                    if (patternSkip.IsMatch(tr.InnerText.ToLower()))
                    {
                        continue;
                    }

                    foreach (HtmlNode a in tr.Descendants("a"))
                    {
                        string href = a.Attributes["href"].Value;
                        string id = Regex.Match(href, @"(nm\d+)").Value;
                        string name = a.InnerText.Trim().RemoveExtraWhitespace();

                        if (dupes.Contains(name))
                        {
                            continue;
                        }

                        dupes.Add(name);

                        crew.Add(new ImdbCrewServiceModel()
                        {
                            ImdbId = id,
                            Name = name,
                            Role = role.ToEnum<CrewRole>()
                        });
                    }
                }
            }

            return crew;
        }

        #endregion Crew

        #region OriginalTitle

        private string GetOriginalTitle()
        {
            HtmlNode node = this.docMain
                .SelectSingleNode("//span[@class='title-extra'][normalize-space(text())]");

            if (node != null)
            {
                return node
                    .SelectSingleNode("//span[@class='title-extra']/text()")
                    .InnerText.Trim();
            }

            return null;
        }

        #endregion OriginalTitle

        #region ReleaseInfo

        private async Task<IEnumerable<ImdbReleaseInfoServiceModel>> GetReleaseInfo(string movieTitleId)
        {
            string contentFilePath = GlobalConstants.ImdbReleaseInfoLogFilePath;
            string releaseInfoUrl = string.Format(GlobalConstants.ImdbReleaseInfoUrl, movieTitleId);

            HtmlNode docReleaseInfo = await GetDocNode(contentFilePath, releaseInfoUrl);

            Regex patternCountries = new Regex(@"Berlin International|Bulgaria|Cannes|Toronto International|USA|Venice");
            Regex patternPremiere = new Regex(@"(?i)Festival|Premiere");
            Regex patternSkip = new Regex(@"(?i)DVD|IMAX|Restored Version|Re-release");
            Regex patternUSA = new Regex(@"(?i)Hollywood|limited|Los Angeles|New York|Sundance");

            var nodes = docReleaseInfo.SelectNodes("//table[@id='release_dates']/tr")
                .Where(m => patternCountries.IsMatch(m.InnerText))
                .Where(m => !patternSkip.IsMatch(m.InnerText));

            var releases = new List<ImdbReleaseInfoServiceModel>();
            foreach (HtmlNode tr in nodes)
            {
                string country = tr.SelectSingleNode(".//td[1]").InnerText.Trim();
                string dateText = tr.SelectSingleNode(".//td[2]").InnerText.Trim();
                string notes = tr.SelectSingleNode(".//td[3]").InnerText.Trim();

                if (country == "USA" && !string.IsNullOrEmpty(notes) && !patternUSA.IsMatch(notes))
                {
                    continue;
                }

                string[] dateElems = dateText
                    .Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (dateElems.Length != 3)
                {
                    dateText = string.Format(dateText, DateAddedValue);
                }

                DateTime dateTime = dateText.ParseToDateTime();

                string location = null;
                ReleaseType releaseType = ReleaseType.General;
                if (!string.IsNullOrEmpty(notes))
                {
                    notes = notes.RemoveExtraWhitespace();
                    notes = Regex.Replace(notes, @"\(|\)", "");
                    notes = Regex.Replace(notes, @"(?i),\s+(?:California|Illinois|Massachusetts|New York|Nevada)", "");
                    //notes = Regex.Replace(notes, @"((?:^|\s|\(|\[|\-|\\|\/)\w)", m => m.Value.ToUpper());

                    string festival = ReleaseType.Festival.ToString().ToLower();
                    string limited = ReleaseType.Limited.ToString().ToLower();
                    string premiere = ReleaseType.Premiere.ToString().ToLower();
                    if (notes.ToLower().Contains(festival))
                    {
                        releaseType = ReleaseType.Festival;
                        location = Regex.Replace(notes, @"(?i)" + premiere, "").Trim();
                    }
                    else if (notes.ToLower().Contains(limited))
                    {
                        releaseType = ReleaseType.Limited;
                        location = Regex.Replace(notes, @"(?i)" + limited, "").Trim();
                    }
                    else if (notes.ToLower().Contains(premiere))
                    {
                        releaseType = ReleaseType.Premiere;
                        location = Regex.Replace(notes, @"(?i)" + premiere, "").Trim();
                    }
                    else
                    {
                        location = notes;
                    }
                }

                releases.Add(new ImdbReleaseInfoServiceModel()
                {
                    Country = country,
                    Date = dateTime,
                    Location = location,
                    Role = releaseType
                });
            }

            // Get fallback release info
            if (releases.Count(r => r.Role == ReleaseType.General) == 0)
            {
                string fallbackReleaseInfo = this.GetZebraListValue("Release Date");

                Match match = Regex.Match(fallbackReleaseInfo, @"(.*?)\s+\((.*?)\)");
                if (match.Success)
                {
                    string dateText = match.Groups[1].Value;
                    string country = match.Groups[2].Value;

                    string[] dateElems = dateText
                        .Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    if (dateElems.Length != 3)
                    {
                        dateText = string.Format(dateText, DateAddedValue);
                    }

                    DateTime dateTime = dateText.ParseToDateTime();

                    releases.Add(new ImdbReleaseInfoServiceModel()
                    {
                        Country = country,
                        Date = dateTime,
                        Role = ReleaseType.General
                    });
                }
            }

            //releases.ForEach(m => Console.WriteLine(m));

            return releases;
        }

        #endregion ReleaseInfo

        #region Runtime

        private int GetRuntime(string value)
        {
            string runtimeValue = this.GetZebraListValue(value);

            if (!string.IsNullOrEmpty(runtimeValue))
            {
                runtimeValue = Regex.Match(runtimeValue, @"\d+").Value;

                return runtimeValue.ParseToInt();
            }

            return 0;
        }

        #endregion Runtime

        #region ProductionYear

        private int GetProductionYear()
        {
            HtmlNode node = this.docMain
                .SelectSingleNode("//h3[.//a[contains(@href, 'year')]]");

            if (node == null)
            {
                throw new InvalidOperationException($"IMDb collecting Production Year failed!");
            }

            //if (Regex.IsMatch(text, @"\p{IsCyrillic}"))
            //{
            //    // there is at least one cyrillic character in the string
            //}

            string productionYearValue = node.Element("span").InnerText.Trim();
            productionYearValue = Regex.Match(productionYearValue, @"\((\d{4})\)").Groups[1].Value;

            return productionYearValue.ParseToInt();
        }

        #endregion ProductionYear

        #region ProductionYearAndTitle

        private (int, string) GetProductionYearAndTitle()
        {
            HtmlNode node = this.docMain
                .SelectSingleNode("//title[contains(text(),'Reference View - IMDb')]");

            if (node == null)
            {
                throw new InvalidOperationException($"IMDb collecting Title failed!");
            }

            string text = node.InnerText.Trim();
            Match match = Regex.Match(text, @"^(.+?)\s\((\d{4})\) - Reference View - IMDb$");
            string title = match.Groups[1].Value;
            int productionYear = match.Groups[2].Value.ParseToInt();

            return (productionYear, title);
        }

        #endregion ProductionYearAndTitle

        #region Studios

        private IEnumerable<ImdbStudioServiceModel> GetStudios()
        {
            var nodes = this.docMain
                .SelectNodes("//header[.//h4[contains(text(), 'Production Companies')]]/following-sibling::ul[1]/li");

            if (nodes == null)
            {
                throw new InvalidOperationException($"IMDb collecting Studios failed!");
            }

            var dupes = new HashSet<string>();
            var productionCompanies = new List<ImdbStudioServiceModel>();
            foreach (var li in nodes)
            {
                string name = li.Element("a").InnerText.Trim().RemoveExtraWhitespace();

                if (dupes.Contains(name))
                {
                    continue;
                }

                dupes.Add(name);

                productionCompanies.Add(new ImdbStudioServiceModel()
                {
                    Name = name,
                    Role = StudioRole.ProductionCompany
                });
            }

            return productionCompanies;
        }

        #endregion Studios

        #region Synopsis

        private string GetSynopsis()
        {
            HtmlNode node = this.docMain
               .SelectSingleNode("//section[@class='titlereference-section-overview']/div[1]/text()");

            if (node == null)
            {
                throw new InvalidOperationException($"IMDb collecting Synopsis failed!");
            }

            return node.InnerText.Trim().RemoveExtraWhitespace();
        }

        #endregion Synopsis

        #region Top250

        private int GetTop250()
        {
            HtmlNode node = this.docMain
                .SelectSingleNode("//a[@href='/chart/top']");

            if (node != null)
            {
                string top250value = node.InnerText.Trim();

                top250value = Regex.Match(top250value, @"#(\d+)").Groups[1].Value;

                return top250value.ParseToInt();
            }

            return 0;
        }

        #endregion Top250

        #region UsersRating

        private double GetUsersRating()
        {
            HtmlNode node = this.docMain
                .SelectSingleNode("//span[@class='ipl-rating-star__rating']/text()");

            if (node == null)
            {
                throw new InvalidOperationException($"IMDb collecting Users rating failed!");
            }

            string usersRating = node.InnerText.Trim();

            return usersRating.ParseToDouble();
        }

        #endregion UsersRating

        #region Common Methods

        private HtmlNode GetRow(string value)
        {
            HtmlNode node = this.docMain
                .SelectSingleNode($"//tr[./td[contains(text(), '{value}')]]");
            if (node == null)
            {
                throw new InvalidOperationException($"IMDb collecting {value} failed!");
            }

            return node;
        }

        private string GetZebraListValue(string value)
        {
            return this.GetRow(value)
                .SelectSingleNode("./td/ul/li/text()[1]")
                .InnerText.Trim();
        }

        private IEnumerable<string> GetZebraListValues(string value)
        {
            IEnumerable<string> values = this.GetRow(value)
                 .Descendants("a")
                 .Where(tag => !tag.InnerText.Contains("See more"))
                 .Select(tag => tag.InnerText.Trim());

            if (value == "Color")
            {
                values = this.ValidateColors(values);
            }
            else if (value == "Country")
            {
                values = this.ValidateCountries(values);
            }

            return values;
        }

        #endregion Common Methods

        #region DownloadPoster

        private void DownloadPoster()
        {
            var node = this.docMain
                 .SelectSingleNode("//img[@id='primary-poster']");

            if (node != null)
            {
                string thumbnailUrl = node.Attributes["src"].Value.Trim();
                string posterUrl = Regex.Replace(thumbnailUrl, @"\d+_", "5000_");
                posterUrl.DownloadFile();
            }
        }

        #endregion DownloadPoster

        #region ValidateColors

        private IEnumerable<string> ValidateColors(IEnumerable<string> colors)
        {
            List<string> c = colors.ToList();
            for (int i = 0; i < c.Count(); i++)
            {
                if (c[i].Contains("Color"))
                {
                    c[i] = "Color";
                }
            }

            return c;
        }

        #endregion ValidateColors

        #region ValidateCountries

        private List<string> ValidateCountries(IEnumerable<string> countries)
        {
            List<string> c = countries.ToList();
            for (int i = 0; i < c.Count(); i++)
            {
                c[i] = c[i].Replace("United States", "USA");
            }

            return c;
        }

        #endregion ValidateCountries
    }
}
