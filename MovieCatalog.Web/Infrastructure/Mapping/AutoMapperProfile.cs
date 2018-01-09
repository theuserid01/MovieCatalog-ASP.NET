namespace MovieCatalog.Web.Infrastructure.Mapping
{
    using System;
    using System.Linq;
    using AutoMapper;
    using MovieCatalog.Common.Mapping;
    using MovieCatalog.Services.Data.Models.Movies;
    using MovieCatalog.Services.Html.Models.Imdb;
    using MovieCatalog.Web.Areas.Movies.Models;

    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            var allTypes = AppDomain
                .CurrentDomain
                .GetAssemblies()
                .Where(a => a.GetName().Name.Contains("MovieCatalog"))
                .SelectMany(a => a.GetTypes());

            allTypes
                .Where(t => t
                    .GetInterfaces()
                    .Where(i => i.IsGenericType)
                    .Any(i => i.GetGenericTypeDefinition() == typeof(IMapFrom<>)))
                .Select(t => new
                {
                    Destination = t,
                    Source = t.GetInterfaces()
                        .Where(i => i.IsGenericType
                            && i.GetGenericTypeDefinition() == typeof(IMapFrom<>))
                        .SelectMany(i => i.GetGenericArguments())
                        .First()
                })
                .ToList()
                .ForEach(mapping => this.CreateMap(mapping.Source, mapping.Destination));

            allTypes
                .Where(t => t.IsClass
                    && !t.IsAbstract
                    && typeof(IHaveCustomMapping).IsAssignableFrom(t))
                .Select(Activator.CreateInstance)
                .Cast<IHaveCustomMapping>()
                .ToList()
                .ForEach(mapping => mapping.ConfigureMapping(this));

            CreateMap<CastFormModel, CastServiceModel>();
            CreateMap<CrewFormModel, CrewServiceModel>();

            CreateMap<ImdbCastServiceModel, CastFormModel>();
            CreateMap<ImdbCrewServiceModel, CrewFormModel>();

            CreateMap<ImdbMainServiceModel, MovieFormMainModel>();
        }
    }
}
