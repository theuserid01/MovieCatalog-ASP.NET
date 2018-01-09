namespace MovieCatalog.Services.Html.Models.RottenTomatoes
{
    using System.Text;

    public class RottenTomatoesMainServiceModel
    {
        public int RottenTomatoesCriticsScore { get; set; }

        public string RottenTomatoesId { get; set; }

        public string RottenTomatoesTitle { get; set; }

        public int RottenTomatoesUsersScore { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("ROTTEN TOMATOES")
                .AppendLine("--------------------------------------------------")
                .AppendLine($"Title: {this.RottenTomatoesTitle}")
                .AppendLine($"Id: {this.RottenTomatoesId}")
                .AppendLine($"Critics Score: {this.RottenTomatoesCriticsScore}%")
                .AppendLine($"Users Score: {this.RottenTomatoesUsersScore}%")
                .AppendLine("--------------------------------------------------");

            return sb.ToString().Trim();
        }
    }
}
