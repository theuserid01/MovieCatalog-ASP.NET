namespace MovieCatalog.Services.Data.Models.Movies
{
    using MovieCatalog.Common.Mapping;
    using MovieCatalog.Data.Models.ManyToMany;

    public class CastServiceModel : ArtistBaseServiceModel, IMapFrom<MovieCast>
    {
        public string Character { get; set; }
    }
}
