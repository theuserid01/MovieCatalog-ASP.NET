namespace MovieCatalog.Services.Data.Models.Movies
{
    using MovieCatalog.Common.Mapping;
    using MovieCatalog.Data.Models.Enums;
    using MovieCatalog.Data.Models.ManyToMany;

    public class CrewServiceModel : ArtistBaseServiceModel, IMapFrom<MovieCrew>
    {
        public CrewRole Role { get; set; }
    }
}
