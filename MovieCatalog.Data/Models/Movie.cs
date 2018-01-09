namespace MovieCatalog.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using MovieCatalog.Data.Models.ManyToMany;
    using static MovieCatalog.Common.DataConstants;

    public class Movie
    {
        public int Id { get; set; }

        [Display(Name = "Box Office Mojo Id")]
        [MinLength(StringMinLength, ErrorMessage = StringMinLengthErrorMessage)]
        [MaxLength(StringMaxLength, ErrorMessage = StringMaxLengthErrorMessage)]
        public string BoxOfficeMojoId { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = NumberRangeToMaxErrorMessage)]
        public decimal Budget { get; set; }

        public DateTime CreationDate { get; set; }

        [Display(Name = "Gross Foreign")]
        [Range(0, double.MaxValue, ErrorMessage = NumberRangeToMaxErrorMessage)]
        public decimal GrossForeign { get; set; }

        [Display(Name = "Gross USA")]
        [Range(0, double.MaxValue, ErrorMessage = NumberRangeToMaxErrorMessage)]
        public decimal GrossUsa { get; set; }

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

        [MinLength(ImageUrlMinLength, ErrorMessage = StringMinLengthErrorMessage)]
        [MaxLength(ImageUrlMaxLength, ErrorMessage = StringMaxLengthErrorMessage)]
        public string PhotoUrl { get; set; }

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

        public int SortIndex { get; set; }

        [MinLength(SynopsisMInLength, ErrorMessage = StringMinLengthErrorMessage)]
        [MaxLength(SynopsisMaxLength, ErrorMessage = StringMinLengthErrorMessage)]
        public string Synopsis { get; set; }

        [MaxLength(ThumbnailMaxLength, ErrorMessage = ImageMaxLengthErrorMessage)]
        public byte[] Thumbnail { get; set; }

        [Required]
        [MinLength(StringMinLength, ErrorMessage = StringMinLengthErrorMessage)]
        [MaxLength(StringMaxLength, ErrorMessage = StringMaxLengthErrorMessage)]
        public string Title { get; set; }

        public string UserId { get; set; }

        public User User { get; set; }

        public List<MovieArtistGoldenGlobe> ArtistsGoldenGlobes { get; set; } = new List<MovieArtistGoldenGlobe>();

        public List<MovieArtistOscar> ArtistsOscars { get; set; } = new List<MovieArtistOscar>();

        public List<MovieColor> Colors { get; set; } = new List<MovieColor>();

        public List<MovieCountry> Countries { get; set; } = new List<MovieCountry>();

        public List<MovieGenre> Genres { get; set; } = new List<MovieGenre>();

        public List<HomeVideo> HomeVideos { get; set; } = new List<HomeVideo>();

        public List<MovieLanguage> Languages { get; set; } = new List<MovieLanguage>();

        public List<MovieCast> MoviesCasts { get; set; } = new List<MovieCast>();

        public List<MovieCrew> MoviesCrews { get; set; } = new List<MovieCrew>();

        public List<Poster> Posters { get; set; } = new List<Poster>();

        public List<Release> Releases { get; set; } = new List<Release>();

        public List<Review> Reviews { get; set; } = new List<Review>();

        public List<MovieStudio> Studios { get; set; } = new List<MovieStudio>();
    }
}
