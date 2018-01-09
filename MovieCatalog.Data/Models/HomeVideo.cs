namespace MovieCatalog.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using MovieCatalog.Data.Models.Enums;
    using MovieCatalog.Data.Models.ManyToMany;
    using static MovieCatalog.Common.DataConstants;

    public class HomeVideo
    {
        public int Id { get; set; }

        [MinLength(BarcodeMInLength, ErrorMessage = StringMinLengthErrorMessage)]
        [MaxLength(BarcodeMaxLength, ErrorMessage = StringMinLengthErrorMessage)]
        public string Barcode { get; set; }

        [MinLength(StringMinLength, ErrorMessage = StringMinLengthErrorMessage)]
        [MaxLength(StringMaxLength, ErrorMessage = StringMaxLengthErrorMessage)]
        public string BlurayDotComId { get; set; }

        public ContentRatingType ContentRating { get; set; }

        public DiscFormatType DiscFormat { get; set; }

        public DiscLayersType DiscLayers { get; set; }

        [Range(DiscTotalMinLength, DiscTotalMaxLength, ErrorMessage = NumberRangeErrorMessage)]
        public int DiscTotal { get; set; }

        [MinLength(StringMinLength, ErrorMessage = StringMinLengthErrorMessage)]
        [MaxLength(StringMaxLength, ErrorMessage = StringMaxLengthErrorMessage)]
        public string Edition { get; set; }

        public bool InCollection { get; set; }

        public DateTime Releasedate { get; set; }

        [Range(0, RuntimeMaxLength, ErrorMessage = NumberRangeErrorMessage)]
        public int Runtime { get; set; }

        [MinLength(SynopsisMInLength, ErrorMessage = StringMinLengthErrorMessage)]
        [MaxLength(SynopsisMaxLength, ErrorMessage = StringMinLengthErrorMessage)]
        public string Synopsis { get; set; }

        [Required]
        [MinLength(StringMinLength, ErrorMessage = StringMinLengthErrorMessage)]
        [MaxLength(StringMaxLength, ErrorMessage = StringMaxLengthErrorMessage)]
        public string Title { get; set; }

        public int CountryId { get; set; }

        public Country Country { get; set; }

        public int MovieId { get; set; }

        public Movie Movie { get; set; }

        public List<HomeVideoSubtitle> Subtitles { get; set; } = new List<HomeVideoSubtitle>();
    }
}
