namespace MovieCatalog.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using static MovieCatalog.Common.DataConstants;

    public class Review
    {
        public int Id { get; set; }

        [Required]
        [MinLength(StringMinLength, ErrorMessage = StringMinLengthErrorMessage)]
        [MaxLength(StringMaxLength, ErrorMessage = StringMaxLengthErrorMessage)]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        [Range(0, ReviewStarsMaxLength, ErrorMessage = NumberRangeErrorMessage)]
        public int Stars { get; set; }

        public string AuthorId { get; set; }

        public User Author { get; set; }

        public int MovieId { get; set; }

        public Movie Movie { get; set; }
    }
}
