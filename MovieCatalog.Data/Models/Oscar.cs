namespace MovieCatalog.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using MovieCatalog.Data.Models.ManyToMany;
    using static MovieCatalog.Common.DataConstants;

    public class Oscar
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(AwardCategoryMaxLength, ErrorMessage = StringMaxLengthErrorMessage)]
        public string Category { get; set; }

        public List<MovieArtistOscar> MoviesArtists { get; set; } = new List<MovieArtistOscar>();
    }
}
