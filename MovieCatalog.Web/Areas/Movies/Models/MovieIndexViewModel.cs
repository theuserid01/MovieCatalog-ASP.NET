namespace MovieCatalog.Web.Areas.Movies.Models
{
    using System.Collections.Generic;
    using MovieCatalog.Services.Data.Models.Movies;

    public class MovieIndexViewModel
    {
        public IEnumerable<MovieBaseServiceModel> AllMovies { get; set; }

        public MovieDetailsServiceModel MovieDetails { get; set; }
    }
}
