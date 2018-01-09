namespace MovieCatalog.Services.Data.Models.Movies
{
    using MovieCatalog.Common.Mapping;
    using MovieCatalog.Data.Models;

    public class ArtistBaseServiceModel : IMapFrom<Artist>
    {
        public int Id { get; set; }

        public string ImdbId { get; set; }

        public string Name { get; set; }

        public string PhotoUrl { get; set; }
    }
}
