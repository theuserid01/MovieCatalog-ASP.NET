namespace MovieCatalog.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using static MovieCatalog.Common.DataConstants;

    public class Location
    {
        public int Id { get; set; }

        [Required]
        [MinLength(StringMinLength, ErrorMessage = StringMinLengthErrorMessage)]
        [MaxLength(StringMaxLength, ErrorMessage = StringMaxLengthErrorMessage)]
        public string Name { get; set; }

        public List<Release> Releases { get; set; } = new List<Release>();
    }
}
