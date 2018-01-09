namespace MovieCatalog.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using MovieCatalog.Data.Models.ManyToMany;
    using static MovieCatalog.Common.DataConstants;

    public class GoldenGlobe
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(AwardCategoryMaxLength, ErrorMessage = StringMaxLengthErrorMessage)]
        public string Category { get; set; }

        public List<MovieArtistGoldenGlobe> MoviesArtists { get; set; } = new List<MovieArtistGoldenGlobe>();
    }
}
