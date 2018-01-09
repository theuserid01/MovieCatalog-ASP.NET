namespace MovieCatalog.Data.Models.ManyToMany
{
    using MovieCatalog.Data.Models.Enums;

    public class MovieCrew
    {
        public int MovieId { get; set; }

        public Movie Movie { get; set; }

        public int ArtistId { get; set; }

        public Artist Artist { get; set; }

        public CrewRole Role { get; set; }
    }
}
