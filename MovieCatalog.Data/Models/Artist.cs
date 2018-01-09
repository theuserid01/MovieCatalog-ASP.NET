namespace MovieCatalog.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using MovieCatalog.Data.Models.ManyToMany;
    using static MovieCatalog.Common.DataConstants;

    public class Artist
    {
        public int Id { get; set; }

        [Required]
        [MinLength(ImdbIdMinLength, ErrorMessage = StringMinLengthErrorMessage)]
        [MaxLength(ImdbIdMaxLength, ErrorMessage = StringMaxLengthErrorMessage)]
        public string ImdbId { get; set; }

        [Required]
        [MinLength(StringMinLength, ErrorMessage = StringMinLengthErrorMessage)]
        [MaxLength(StringMaxLength, ErrorMessage = StringMaxLengthErrorMessage)]
        public string Name { get; set; }

        [MinLength(ImageUrlMinLength, ErrorMessage = StringMinLengthErrorMessage)]
        [MaxLength(ImageUrlMaxLength, ErrorMessage = StringMaxLengthErrorMessage)]
        public string PhotoUrl { get; set; }

        public List<MovieCast> MoviesCasts { get; set; } = new List<MovieCast>();

        public List<MovieCrew> MoviesCrews { get; set; } = new List<MovieCrew>();

        public List<MovieArtistGoldenGlobe> MoviesGoldenGlobes { get; set; } = new List<MovieArtistGoldenGlobe>();

        public List<MovieArtistOscar> MoviesOscars { get; set; } = new List<MovieArtistOscar>();

        public List<Photo> Photos { get; set; } = new List<Photo>();
    }
}
