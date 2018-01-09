namespace MovieCatalog.Web.Areas.Movies.Models
{
    using System.ComponentModel.DataAnnotations;
    using static MovieCatalog.Common.DataConstants;

    public class HomeVideoFormModel
    {
        [MinLength(SynopsisMInLength, ErrorMessage = StringMinLengthErrorMessage)]
        [MaxLength(SynopsisMaxLength, ErrorMessage = StringMinLengthErrorMessage)]
        public string Synopsis { get; set; }
    }
}
