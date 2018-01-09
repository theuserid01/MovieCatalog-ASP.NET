namespace MovieCatalog.Services.Html.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class SearchTitleBaseServiceModel
    {
        public IList<KeyValuePair<string, string>> SearchTitleResults { get; set; }
            = new List<KeyValuePair<string, string>>();

        public override string ToString()
        {
            var results = this.SearchTitleResults
                .Select(kvp => $"{kvp.Key} => {kvp.Value}");

            return string.Join(Environment.NewLine, results);
        }
    }
}
