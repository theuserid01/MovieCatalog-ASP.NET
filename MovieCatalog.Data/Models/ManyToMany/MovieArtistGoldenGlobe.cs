namespace MovieCatalog.Data.Models.ManyToMany
{
    using System.ComponentModel.DataAnnotations;
    using MovieCatalog.Data.Models.Enums;
    using static MovieCatalog.Common.DataConstants;

    public class MovieArtistGoldenGlobe
    {
        public int MovieId { get; set; }

        public Movie Movie { get; set; }

        public int ArtistId { get; set; }

        public Artist Artist { get; set; }

        public int GoldenGlobeId { get; set; }

        public GoldenGlobe GoldenGlobe { get; set; }

        public AwardRole Role { get; set; }

        [MaxLength(AwardNotesMaxLength, ErrorMessage = StringMaxLengthErrorMessage)]
        public string Notes { get; set; }

        [Range(AwardYearMinLength, AwardYearMaxLength)]
        public int Year { get; set; }
    }
}
