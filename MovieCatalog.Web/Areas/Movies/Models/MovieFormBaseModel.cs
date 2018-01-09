namespace MovieCatalog.Web.Areas.Movies.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class MovieFormBaseModel
    {
        public IEnumerable<SelectListItem> AllColors { get; set; }

        public IEnumerable<SelectListItem> AllCountries { get; set; }

        public IEnumerable<SelectListItem> AllGenres { get; set; }

        public IEnumerable<SelectListItem> AllLanguages { get; set; }

        [Display(Name = "Colors")]
        public IEnumerable<int> SelectedColors { get; set; }

        [Display(Name = "Countries")]
        public IEnumerable<int> SelectedCountries { get; set; }

        [Display(Name = "Genres")]
        public IEnumerable<int> SelectedGenres { get; set; }

        [Display(Name = "Languages")]
        public IEnumerable<int> SelectedLanguages { get; set; }
    }
}
