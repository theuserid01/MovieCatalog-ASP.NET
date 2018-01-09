namespace MovieCatalog.Data.Models
{
    using System;
    using MovieCatalog.Data.Models.Enums;

    public class Release
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public ReleaseType Role { get; set; }

        public int CountryId { get; set; }

        public Country Country { get; set; }

        public int LocationId { get; set; }

        public Location Location { get; set; }

        public int MovieId { get; set; }

        public Movie Movie { get; set; }
    }
}
