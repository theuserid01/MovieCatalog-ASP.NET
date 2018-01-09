namespace MovieCatalog.Web.Areas.Movies.Models
{
    using System.ComponentModel.DataAnnotations;
    using static MovieCatalog.Common.DataConstants;

    public class CastFormModel : ArtistFormBaseModel
    {
        [Required]
        [MinLength(StringMinLength, ErrorMessage = StringMinLengthErrorMessage)]
        [MaxLength(StringMaxLength, ErrorMessage = StringMaxLengthErrorMessage)]
        public string Character { get; set; }
    }
}
