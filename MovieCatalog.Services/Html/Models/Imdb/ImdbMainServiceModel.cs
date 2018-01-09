namespace MovieCatalog.Services.Html.Models.Imdb
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class ImdbMainServiceModel
    {
        public IDictionary<string, IList<ImdbAwardServiceModel>> Awards { get; set; }

        public IEnumerable<ImdbCastServiceModel> Cast { get; set; }

        public IEnumerable<string> Colors { get; set; }

        public IEnumerable<string> Countries { get; set; }

        public IEnumerable<ImdbCrewServiceModel> Crew { get; set; }

        public IEnumerable<string> Genres { get; set; }

        public IEnumerable<string> Languages { get; set; }

        public string ImdbId { get; set; }

        public int ImdbTop250 { get; set; }

        public double ImdbUsersRating { get; set; }

        public string OriginalTitle { get; set; }

        public int ProductionYear { get; set; }

        public IEnumerable<ImdbReleaseInfoServiceModel> Releases { get; set; }

        public int Runtime { get; set; }

        public IEnumerable<ImdbStudioServiceModel> Studios { get; set; }

        public string Synopsis { get; set; }

        public string Title { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("IMDb")
                .AppendLine("--------------------------------------------------")
                .AppendLine($"Title: {this.Title}")
                .AppendLine($"Color: {string.Join(", ", this.Colors)}")
                .AppendLine($"Countries: {string.Join(", ", this.Countries)}")
                .AppendLine($"Genres: {string.Join(", ", this.Genres)}")
                .AppendLine($"Languages: {string.Join(", ", this.Languages)}")
                .AppendLine($"ImdbId: {this.ImdbId}")
                .AppendLine($"ImdbTop250: {this.ImdbTop250}")
                .AppendLine($"ImdbUsersRating: {this.ImdbUsersRating}")
                .AppendLine($"Original Title: {this.OriginalTitle}")
                .AppendLine($"Production Year: {this.ProductionYear}")
                .AppendLine($"Runtime: {this.Runtime}")
                .AppendLine()
                .AppendLine($"Studios:")
                .AppendLine(string.Join(Environment.NewLine, this.Studios))
                .AppendLine()
                .AppendLine($"Synopsis:")
                .AppendLine(this.Synopsis)
                .AppendLine()
                .AppendLine("Cast:")
                .AppendLine(string.Join(Environment.NewLine, this.Cast))
                .AppendLine()
                .AppendLine("Crew:")
                .AppendLine(string.Join(Environment.NewLine, this.Crew))
                .AppendLine()
                .AppendLine("Release Info:")
                .AppendLine(string.Join(Environment.NewLine, this.Releases));

            foreach (var kvp in this.Awards)
            {
                sb.AppendLine()
                    .AppendLine(kvp.Key);
                foreach (var award in kvp.Value)
                {
                    sb.AppendLine($"{award}");
                }
            }

            sb.AppendLine("--------------------------------------------------");

            return sb.ToString().Trim();
        }
    }
}
