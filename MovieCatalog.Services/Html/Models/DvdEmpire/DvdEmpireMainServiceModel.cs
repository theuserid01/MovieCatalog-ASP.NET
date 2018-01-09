namespace MovieCatalog.Services.Html.Models.DvdEmpire
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class DvdEmpireMainServiceModel
    {
        public string Barcode { get; set; }

        public string ContentRating { get; set; }

        public string DiscLayers { get; set; }

        public int DiscTotal { get; set; }

        public string Distributor { get; set; }

        public string DvdEmpireId { get; set; }

        public int ProductionYear { get; set; }

        public DateTime ReleaseDate { get; set; }

        public int Runtime { get; set; }

        public string Specialfeatures { get; set; }

        public IEnumerable<string> Subtitles { get; set; }

        public string Synopsis { get; set; }

        public string Title { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("DVD EMPIRE")
                .AppendLine("--------------------------------------------------")
                .AppendLine($"Title: {this.Title}")
                .AppendLine($"Id: {this.DvdEmpireId}")
                .AppendLine($"Barcode: {this.Barcode}")
                .AppendLine($"Content Rating: {this.ContentRating}")
                .AppendLine($"Disc Layers: {this.DiscLayers}")
                .AppendLine($"Disc Total: {this.DiscTotal}")
                .AppendLine($"Distributor: {this.Distributor}")
                .AppendLine($"Production Year: {this.ProductionYear}")
                .AppendLine($"Release date: {this.ReleaseDate.ToShortDateString()}")
                .AppendLine($"Runtime: {this.Runtime} min")
                .AppendLine($"Subtitles: {string.Join(", ", this.Subtitles)}")
                .AppendLine()
                .AppendLine("Special Features:")
                .AppendLine(this.Specialfeatures)
                .AppendLine()
                .AppendLine("Synopsis:")
                .AppendLine(this.Synopsis)
                .AppendLine("--------------------------------------------------");

            return sb.ToString().Trim();
        }
    }
}
