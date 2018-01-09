namespace MovieCatalog.Web.Areas.Movies.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using static MovieCatalog.Common.DataConstants;

    public class MovieFormMainModel : MovieFormBaseModel
    {
        [Display(Name = "Box Office Mojo Id")]
        [MinLength(StringMinLength, ErrorMessage = StringMinLengthErrorMessage)]
        [MaxLength(StringMaxLength, ErrorMessage = StringMaxLengthErrorMessage)]
        public string BoxOfficeMojoId { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = NumberRangeToMaxErrorMessage)]
        public decimal Budget { get; set; }

        public List<CastFormModel> Cast { get; set; } = new List<CastFormModel>();

        public List<CrewFormModel> Crew { get; set; } = new List<CrewFormModel>();

        [Display(Name = "Gross Foreign")]
        [Range(0, double.MaxValue, ErrorMessage = NumberRangeToMaxErrorMessage)]
        public decimal GrossForeign { get; set; }

        [Display(Name = "Gross USA")]
        [Range(0, double.MaxValue, ErrorMessage = NumberRangeToMaxErrorMessage)]
        public decimal GrossUsa { get; set; }

        public HomeVideoFormModel HomeVideo { get; set; }

        [Display(Name = "IMDb Id")]
        [MinLength(ImdbIdMinLength, ErrorMessage = StringMinLengthErrorMessage)]
        [MaxLength(ImdbIdMaxLength, ErrorMessage = StringMaxLengthErrorMessage)]
        public string ImdbId { get; set; }

        [Display(Name = "IMDb Top 250")]
        [Range(0, ImdbTop250MaxLength, ErrorMessage = NumberRangeErrorMessage)]
        public int ImdbTop250 { get; set; }

        [Display(Name = "IMDb Users Rating")]
        [Range(0, ImdbUsersRatingMaxLength, ErrorMessage = NumberRangeErrorMessage)]
        public double ImdbUsersRating { get; set; }

        [Display(Name = "Original Title")]
        [MinLength(StringMinLength, ErrorMessage = StringMinLengthErrorMessage)]
        [MaxLength(StringMaxLength, ErrorMessage = StringMaxLengthErrorMessage)]
        public string OriginalTitle { get; set; }

        [Display(Name = "Production Year")]
        [Range(ProductionYearMinLength, ProductionYearMaxLength)]
        public int ProductionYear { get; set; }

        [Display(Name = "Rotten Tomatoes Critics Score")]
        [Range(0, RottenTomattoesScoreMaxLength, ErrorMessage = NumberRangeErrorMessage)]
        public int RottenTomatoesCriticsScore { get; set; }

        [Display(Name = "Rotten Tomatoes Id")]
        [MinLength(StringMinLength, ErrorMessage = StringMinLengthErrorMessage)]
        [MaxLength(StringMaxLength, ErrorMessage = StringMaxLengthErrorMessage)]
        public string RottenTomatoesId { get; set; }

        [Display(Name = "Rotten Tomatoes Users Score")]
        [Range(0, RottenTomattoesScoreMaxLength, ErrorMessage = NumberRangeErrorMessage)]
        public int RottenTomatoesUsersScore { get; set; }

        [Range(0, RuntimeMaxLength, ErrorMessage = NumberRangeErrorMessage)]
        public int Runtime { get; set; }

        [MinLength(SynopsisMInLength, ErrorMessage = StringMinLengthErrorMessage)]
        [MaxLength(SynopsisMaxLength, ErrorMessage = StringMinLengthErrorMessage)]
        public string Synopsis { get; set; }

        public IEnumerable<byte[]> Posters { get; set; }

        [Required]
        [MinLength(StringMinLength, ErrorMessage = StringMinLengthErrorMessage)]
        [MaxLength(StringMaxLength, ErrorMessage = StringMaxLengthErrorMessage)]
        public string Title { get; set; }
    }
}
