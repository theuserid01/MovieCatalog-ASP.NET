namespace MovieCatalog.Services.Data.Models.Movies
{
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper;
    using MovieCatalog.Common.Mapping;
    using MovieCatalog.Data.Models;

    public class MovieBaseServiceModel : IMapFrom<Movie>, IHaveCustomMapping
    {
        public int Id { get; set; }

        public IEnumerable<GenreServiceModel> Genres { get; set; }

        public int ProductionYear { get; set; }

        public byte[] Thumbnail { get; set; }

        public string Title { get; set; }

        public virtual void ConfigureMapping(Profile mapper)
        {
            mapper.CreateMap<Movie, MovieBaseServiceModel>()
                .Include<Movie, MovieDetailsServiceModel>()
                .ForMember(m => m.Genres, cfg => cfg
                    .MapFrom(mg => mg.Genres.Select(m =>
                        new GenreServiceModel() { Id = m.Genre.Id, Name = m.Genre.Name })));
        }
    }
}
