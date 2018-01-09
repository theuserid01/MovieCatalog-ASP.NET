namespace MovieCatalog.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using MovieCatalog.Data.Models.ManyToMany;
    using static MovieCatalog.Common.DataConstants;

    public class Color
    {
        public int Id { get; set; }

        [Required]
        [MinLength(ColorMinLength, ErrorMessage = StringMinLengthErrorMessage)]
        [MaxLength(ColorMaxLength, ErrorMessage = StringMaxLengthErrorMessage)]
        public string Name { get; set; }

        public List<MovieColor> Movies { get; set; } = new List<MovieColor>();
    }
}
