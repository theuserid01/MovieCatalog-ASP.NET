using System.Text;

namespace MovieCatalog.Services.Html.Models.BoxOfficeMojo
{
    public class BoxOfficeMojoMainServiceModel
    {
        public string BoxOfficeMojoId { get; set; }

        public string BoxOfficeMojoTitle { get; set; }

        public decimal Budget { get; set; }

        public decimal GrossForeign { get; set; }

        public decimal GrossUsa { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("BOX OFFICE MOJO")
                .AppendLine("--------------------------------------------------")
                .AppendLine($"Title: {this.BoxOfficeMojoTitle}")
                .AppendLine($"Id: {this.BoxOfficeMojoId}")
                .AppendLine($"Budget: {this.Budget:N0}")
                .AppendLine($"Gross Foreign: {this.GrossForeign:N0}")
                .AppendLine($"Gross USA: {this.GrossUsa:N0}")
                .AppendLine("--------------------------------------------------");

            return sb.ToString().Trim();
        }
    }
}
