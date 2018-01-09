namespace MovieCatalog.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using MovieCatalog.Data.Models.ManyToMany;
    using static MovieCatalog.Common.DataConstants;

    public class Language
    {
        public int Id { get; set; }

        [Required]
        [MinLength(StringMinLength, ErrorMessage = StringMinLengthErrorMessage)]
        [MaxLength(StringMaxLength, ErrorMessage = StringMaxLengthErrorMessage)]
        public string Name { get; set; }

        public List<HomeVideoSubtitle> HomeVideos { get; set; } = new List<HomeVideoSubtitle>();

        public List<MovieLanguage> Movies { get; set; } = new List<MovieLanguage>();
    }
}
