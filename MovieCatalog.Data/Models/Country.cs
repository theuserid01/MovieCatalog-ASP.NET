namespace MovieCatalog.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using MovieCatalog.Data.Models.ManyToMany;
    using static MovieCatalog.Common.DataConstants;

    public class Country
    {
        public int Id { get; set; }

        [Required]
        [MinLength(CountryCodeMinLength, ErrorMessage = StringMinLengthErrorMessage)]
        [MaxLength(CountryCodeMaxLength, ErrorMessage = StringMaxLengthErrorMessage)]
        public string Code { get; set; }

        [Required]
        [MinLength(StringMinLength, ErrorMessage = StringMinLengthErrorMessage)]
        [MaxLength(StringMaxLength, ErrorMessage = StringMaxLengthErrorMessage)]
        public string Name { get; set; }

        [Required]
        [MinLength(StringMinLength, ErrorMessage = StringMinLengthErrorMessage)]
        [MaxLength(StringMaxLength, ErrorMessage = StringMaxLengthErrorMessage)]
        public string OfficialName { get; set; }

        public List<HomeVideo> HomeVideos { get; set; } = new List<HomeVideo>();

        public List<Release> Releases { get; set; } = new List<Release>();

        public List<MovieCountry> Movies { get; set; } = new List<MovieCountry>();
    }
}
