namespace MovieCatalog.Data.Models
{
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Identity;

    public class User : IdentityUser
    {
        public List<Movie> Movies { get; set; } = new List<Movie>();

        public List<Review> Reviews { get; set; } = new List<Review>();
    }
}
