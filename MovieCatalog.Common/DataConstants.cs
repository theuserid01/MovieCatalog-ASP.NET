namespace MovieCatalog.Common
{
    public class DataConstants
    {
        public const int AwardCategoryMaxLength = 200;
        public const int AwardNotesMaxLength = 200;
        public const int AwardYearMaxLength = 3000;
        public const int AwardYearMinLength = 1929;
        public const int BarcodeMInLength = 12;
        public const int BarcodeMaxLength = 13;
        public const int ColorMaxLength = 15;
        public const int ColorMinLength = 5;
        public const int CountryCodeMaxLength = 2;
        public const int CountryCodeMinLength = 2;
        public const int DiscTotalMaxLength = 100;
        public const int DiscTotalMinLength = 1;
        public const int ImageUrlMaxLength = 2000;
        public const int ImageUrlMinLength = 10;
        public const int ImdbIdMaxLength = 9;
        public const int ImdbIdMinLength = 9;
        public const int ImdbTop250MaxLength = 250;
        public const double ImdbUsersRatingMaxLength = 10.0;
        public const int ReviewStarsMaxLength = 10;
        public const int RottenTomattoesScoreMaxLength = 100;
        public const int RuntimeMaxLength = 100_000;
        public const int StringMaxLength = 100;
        public const int StringMinLength = 1;
        public const int SynopsisMaxLength = 10_000;
        public const int SynopsisMInLength = 3;
        public const int PhotoMaxLength = 20 * 1024 * 1024;  // 20MB
        public const int PosterMaxLength = 20 * 1024 * 1024;  // 20MB
        public const int ProductionYearMaxLength = 3000;
        public const int ProductionYearMinLength = 1900;
        public const int ThumbnailHeightPixels = 300;
        public const int ThumbnailMaxLength = 1 * 1024 * 1024;  // 1MB

        // Error Messages
        public const string ImageMaxLengthErrorMessage = "File size should be less than {1}MB.";

        public const string NumberRangeErrorMessage = "{0} value should be between {1} and {2}";

        public const string NumberRangeToMaxErrorMessage = "{0} value should be greater than {1}";

        public const string StringMaxLengthErrorMessage = "{0} should be max {1} symbols long.";

        public const string StringMinLengthErrorMessage = "{0} should be min {1} symbols long.";

        public static string[] AllowedImageExtensions = new string[]
                                                { ".bmp", ".gif", ".jpg", ".jpeg", ".png", ".tif", ".tiff" };
    }
}
