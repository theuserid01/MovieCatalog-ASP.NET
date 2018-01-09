namespace MovieCatalog.Common
{
    public class GlobalConstants
    {
        // Admin Credentials
        public const string AdminEmail = "admin@admin.com";

        public const string AdminPassword = "Admin1";
        public const string AdminUsername = "admin";

        // Misc
        public const int PageSize = 3;

        public const int ImdbCastNumber = 15;

        // Roles
        public const string AdministratorRole = "Administrator";

        public const string ModeratorRole = "Moderator";

        public const int PasswordMinLength = 6;
        public const int PasswordMaxLength = 100;
        public const int UsernameMinLength = 3;
        public const int UsernameMaxLength = 100;

        // Paths
        public const string LogExceptionsDir = "../Local/Exceptions";

        public const string ImdbAwardsLogFilePath = "../Local/Logs/ImdbAwards.html";
        public const string ImdbReleaseInfoLogFilePath = "../Local/Logs/ImdbReleaseInfo.html";

        public const string MainContentBlurayDotComLogFilePath = "../Local/Logs/MainContentBlurayDotCom.html";
        public const string MainContentBoxOfficeMojoLogFilePath = "../Local/Logs/MainContentBoxOfficeMojo.html";
        public const string MainContentDvdEmpireLogFilePath = "../Local/Logs/MainContentDvdEmpire.html";
        public const string MainContentImdbLogFilePath = "../Local/Logs/MainContentImdb.html";
        public const string MainContentRottenTomatoesLogFilePath = "../Local/Logs/MainContentRottenTomatoes.html";

        public const string SearchResultsBlurayDotComLogFilePath = "../Local/Logs/SearchResultsBlurayDotCom.html";
        public const string SearchResultsBoxOfficeMojoLogFilePath = "../Local/Logs/SearchResultsBoxOfficeMojo.html";
        public const string SearchResultsDvdEmpireLogIlePath = "../Local/Logs/SearchResultsDvdEmpire.html";
        public const string SearchResultsImdbLogFilePath = "../Local/Logs/SearchResultsImdb.html";
        public const string SearchResultsRottenTomatoesLogFilePath = "../Local/Logs/SearchResultsRottenTomattoes.html";

        public const string CountriesSqlFilePath = "../Local/Sql/Countries.sql";
        public const string ColorsSqlFilePath = "../Local/Sql/Colors.sql";
        public const string GenresSqlFilePath = "../Local/Sql/Genres.sql";
        public const string GoldenGlobesSqlFilePath = "../Local/Sql/GoldenGlobes.sql";
        public const string LanguagesSqlFilePath = "../Local/Sql/Languages.sql";
        public const string LocationsSqlFilePath = "../Local/Sql/Locations.sql";
        public const string OscarsSqlFilePath = "../Local/Sql/Oscars.sql";

        public const string TempHtmlFilePath = "../Local/Temp/Temp.html";
        public const string TempPosterFilePath = "../Local/Temp/Poster.jpg";
        public const string TempTextFilePath = "../Local/Temp/Temp.txt";
        public const string TempThumbnailFilePath = "../Local/Temp/Thumbnail.jpg";

        // Url
        public const string BlurayDotComDomain = "http://www.blu-ray.com";

        public const string BlurayDotComMainContentUrl = "http://www.blu-ray.com/movies/{0}";
        public const string BlurayDotComSearchTitleUrl = "http://www.blu-ray.com/search/?quicksearch=1&quicksearch_country=US&quicksearch_keyword={0}&section=bluraymovies";

        public const string BoxOfficeMojoDomain = "http://www.boxofficemojo.com";
        public const string BoxOfficeMojoMainContentUrl = "http://www.boxofficemojo.com/movies/?id={0}";
        public const string BoxOfficeMojoSearchTitleUrl = "http://www.boxofficemojo.com/search/?q={0}";

        public const string DvdEmpireDomain = "https://www.dvdempire.com";
        public const string DvdEmpireMainContentUrl = "https://www.dvdempire.com/{0}";
        public const string DvdEmpireSearchTileUrl = "https://www.dvdempire.com/blu-ray/search?q={0}";

        public const string ImdbDomain = "http://www.imdb.com";
        public const string ImdbAwardsUrl = "http://www.imdb.com/title/{0}/awards";
        public const string ImdbMainContentUrl = "http://www.imdb.com/title/{0}/reference";
        public const string ImdbReleaseInfoUrl = "http://www.imdb.com/title/{0}/releaseinfo";
        public const string ImdbSearchTitleUrl = "http://www.imdb.com/find?ref_=nv_sr_fn&q={0}&s=tt";

        public const string RottenTomatoesDomain = "https://www.rottentomatoes.com";
        public const string RottenTomatoesMainContentUrl = "https://www.rottentomatoes.com/{0}";
        public const string RottenTomatoesSearchTitleUrl = "https://www.rottentomatoes.com/search/?search={0}";
    }
}
