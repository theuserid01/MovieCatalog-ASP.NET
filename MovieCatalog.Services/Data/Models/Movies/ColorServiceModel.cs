﻿namespace MovieCatalog.Services.Data.Models.Movies
{
    using MovieCatalog.Common.Mapping;
    using MovieCatalog.Data.Models;

    public class ColorServiceModel : IMapFrom<Color>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
