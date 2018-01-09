namespace MovieCatalog.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using static MovieCatalog.Common.DataConstants;

    public class Photo
    {
        public int Id { get; set; }

        [MaxLength(PhotoMaxLength, ErrorMessage = ImageMaxLengthErrorMessage)]
        public byte[] Image { get; set; }

        public int ArtistId { get; set; }

        public Artist Artist { get; set; }
    }
}
