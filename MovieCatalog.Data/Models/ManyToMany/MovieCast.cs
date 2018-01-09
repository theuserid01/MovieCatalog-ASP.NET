namespace MovieCatalog.Data.Models.ManyToMany
{
    using System.ComponentModel.DataAnnotations;
    using static MovieCatalog.Common.DataConstants;

    public class MovieCast
    {
        public int MovieId { get; set; }

        public Movie Movie { get; set; }

        public int ArtistId { get; set; }

        public Artist Artist { get; set; }

        [Required]
        [MinLength(StringMinLength, ErrorMessage = StringMinLengthErrorMessage)]
        [MaxLength(StringMaxLength, ErrorMessage = StringMaxLengthErrorMessage)]
        public string Character { get; set; }
    }
}
