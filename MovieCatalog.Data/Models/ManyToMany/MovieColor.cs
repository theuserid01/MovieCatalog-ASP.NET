namespace MovieCatalog.Data.Models.ManyToMany
{
    public class MovieColor
    {
        public int MovieId { get; set; }

        public Movie Movie { get; set; }

        public int ColorId { get; set; }

        public Color Color { get; set; }
    }
}
