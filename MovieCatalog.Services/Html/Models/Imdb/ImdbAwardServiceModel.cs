namespace MovieCatalog.Services.Html.Models.Imdb
{
    using System.Collections.Generic;
    using MovieCatalog.Data.Models.Enums;

    public class ImdbAwardServiceModel
    {
        public string Category { get; set; }

        public string Notes { get; set; }

        public IEnumerable<string> Recipients { get; set; }

        public AwardRole Role { get; set; }

        public int Year { get; set; }

        public override string ToString()
            => $"{this.Role} {this.Category} {string.Join(", ", this.Recipients)}";
    }
}
