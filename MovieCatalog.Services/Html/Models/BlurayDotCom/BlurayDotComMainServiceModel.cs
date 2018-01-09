namespace MovieCatalog.Services.Html.Models.BlurayDotCom
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class BlurayDotComMainServiceModel
    {
        public string AspectRatio { get; set; }

        public string BlurayDotComId { get; set; }

        public string ContentRating { get; set; }

        public string DiscLayers { get; set; }

        public int DiscTotal { get; set; }

        public string Distributor { get; set; }

        public string OriginalAspectRatio { get; set; }

        public int ProductionYear { get; set; }

        public DateTime ReleaseDate { get; set; }

        public string Resolution { get; set; }

        public int Runtime { get; set; }

        public IEnumerable<string> Subtitles { get; set; }

        public string Title { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("BLU-RAY.COM")
                .AppendLine("--------------------------------------------------")
                .AppendLine($"Title: {this.Title}")
                .AppendLine($"Id: {this.BlurayDotComId}")
                .AppendLine($"Aspect Ratio: {this.AspectRatio}")
                .AppendLine($"Audio Tracks: TODO")
                .AppendLine($"Content Rating: {this.ContentRating}")
                .AppendLine($"Disc Layers: TODO")
                .AppendLine($"Disc Total: TODO")
                .AppendLine($"Distributor: {this.Distributor}")
                .AppendLine($"Original Aspect Ratio: {this.OriginalAspectRatio}")
                .AppendLine($"Production Year: {this.ProductionYear}")
                .AppendLine($"Release date: {this.ReleaseDate.ToShortDateString()}")
                .AppendLine($"Resolution: {this.Resolution}")
                .AppendLine($"Runtime: {this.Runtime} min")
                .AppendLine($"Subtitles: {string.Join(", ", this.Subtitles)}")
                .AppendLine("--------------------------------------------------");

            return sb.ToString().Trim();
        }
    }
}
