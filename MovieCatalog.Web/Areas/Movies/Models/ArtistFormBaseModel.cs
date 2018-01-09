namespace MovieCatalog.Web.Areas.Movies.Models
{
    using System.ComponentModel.DataAnnotations;
    using static MovieCatalog.Common.DataConstants;

    public class ArtistFormBaseModel
    {
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
    }
}
