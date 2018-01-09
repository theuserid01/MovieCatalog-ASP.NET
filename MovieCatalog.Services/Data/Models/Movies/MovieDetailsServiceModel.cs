namespace MovieCatalog.Services.Data.Models.Movies
{
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper;
    using MovieCatalog.Data.Models;

    public class MovieDetailsServiceModel : MovieBaseServiceModel
    {
        public string BoxOfficeMojoId { get; set; }

        public decimal Budget { get; set; }

        public decimal GrossForeign { get; set; }

        public decimal GrossUsa { get; set; }

        public string ImdbId { get; set; }

        public int ImdbTop250 { get; set; }

        public double ImdbUsersRating { get; set; }

        public string OriginalTitle { get; set; }

        public int RottenTomatoesCriticsScore { get; set; }

        public string RottenTomatoesId { get; set; }

        public int RottenTomatoesUsersScore { get; set; }

        public int Runtime { get; set; }

        public IEnumerable<CastServiceModel> Cast { get; set; }

        public IEnumerable<CrewServiceModel> Crew { get; set; }

        public IEnumerable<ColorServiceModel> Colors { get; set; }

        public IEnumerable<CountryServiceModel> Countries { get; set; }

        public IEnumerable<LanguageServiceModel> Languages { get; set; }

        public string Synopsis { get; set; }

        public override void ConfigureMapping(Profile mapper)
        {
            mapper.CreateMap<Movie, MovieDetailsServiceModel>()
                .ForMember(dest => dest.Cast, cfg => cfg
                    .MapFrom(source => source.MoviesCasts.Select(mc =>
                        new CastServiceModel() { Character = mc.Character, ImdbId = mc.Artist.ImdbId, Name = mc.Artist.Name })))
                 .ForMember(dest => dest.Crew, cfg => cfg
                    .MapFrom(source => source.MoviesCrews.Select(mc =>
                        new CrewServiceModel() { ImdbId = mc.Artist.ImdbId, Name = mc.Artist.Name, Role = mc.Role })))
                .ForMember(dest => dest.Colors, cfg => cfg
                    .MapFrom(mc => mc.Colors.Select(m =>
                        new ColorServiceModel() { Id = m.Color.Id, Name = m.Color.Name })))
                .ForMember(dest => dest.Countries, cfg => cfg
                    .MapFrom(mc => mc.Countries.Select(m =>
                        new CountryServiceModel() { Id = m.Country.Id, Name = m.Country.Name })))
                .ForMember(dest => dest.Languages, cfg => cfg
                    .MapFrom(ml => ml.Languages.Select(m =>
                        new LanguageServiceModel() { Id = m.Language.Id, Name = m.Language.Name })));

            base.ConfigureMapping(mapper);
        }
    }
}
