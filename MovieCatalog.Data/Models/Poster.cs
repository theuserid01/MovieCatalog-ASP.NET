namespace MovieCatalog.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using static MovieCatalog.Common.DataConstants;

    public class Poster
    {
        public int Id { get; set; }

        [MaxLength(PosterMaxLength, ErrorMessage = ImageMaxLengthErrorMessage)]
        public byte[] Image { get; set; }

        public int MovieId { get; set; }

        public Movie Movie { get; set; }
    }
}
