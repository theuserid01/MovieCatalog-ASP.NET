namespace MovieCatalog.Data.Models.ManyToMany
{
    using MovieCatalog.Data.Models.Enums;

    public class MovieStudio
    {
        public int MovieId { get; set; }

        public Movie Movie { get; set; }

        public int StudioId { get; set; }

        public Studio Studio { get; set; }

        public StudioRole Role { get; set; }
    }
}
