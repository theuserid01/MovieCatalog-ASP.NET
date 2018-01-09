namespace MovieCatalog.Web.Areas.Movies.Models
{
    using MovieCatalog.Data.Models.Enums;

    public class CrewFormModel : ArtistFormBaseModel
    {
        public CrewRole Role { get; set; }
    }
}
