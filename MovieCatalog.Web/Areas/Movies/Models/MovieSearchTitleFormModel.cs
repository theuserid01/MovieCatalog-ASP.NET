namespace MovieCatalog.Web.Areas.Movies.Models
{
    using System.Collections.Generic;

    public class MovieSearchTitleFormModel
    {
        public IEnumerable<KeyValuePair<string, string>> BoxOfficeMojoSearchTitleResults { get; set; }

        public IEnumerable<KeyValuePair<string, string>> BlurayDoComSearchTitleResults { get; set; }

        public IEnumerable<KeyValuePair<string, string>> DvdEmpireSearchTitleResults { get; set; }

        public IEnumerable<KeyValuePair<string, string>> ImdbSearchTitleResults { get; set; }

        public IEnumerable<KeyValuePair<string, string>> RottenTomatoesSearchTitleResults { get; set; }

        public string BlurayDotComExceptionMessage { get; set; }

        public string BoxOfficeMojoExceptionMessage { get; set; }

        public string DvdEmpireExceptionMessage { get; set; }

        public string ImdbExceptionMessage { get; set; }

        public string RottentTomattoesExceptionMessage { get; set; }

        public string BoxOfficeMojoSelectedTitle { get; set; }

        public string BlurayDotComSelectedTitle { get; set; }

        public string DvdEmpireSelectedTitle { get; set; }

        public string ImdbSelectedTitle { get; set; }

        public string RottenTomattoesSelectedTitle { get; set; }

        public string SearchTitleText { get; set; }
    }
}
