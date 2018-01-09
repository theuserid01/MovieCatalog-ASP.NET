namespace MovieCatalog.Services.Html.Models.Imdb
{
    using System;
    using MovieCatalog.Data.Models.Enums;

    public class ImdbReleaseInfoServiceModel
    {
        public string Country { get; set; }

        public DateTime Date { get; set; }

        public string Location { get; set; }

        public ReleaseType Role { get; set; }

        public override string ToString()
            => $"{this.Role} {this.Date.ToShortDateString()} {this.Country} {this.Location}";
    }
}
