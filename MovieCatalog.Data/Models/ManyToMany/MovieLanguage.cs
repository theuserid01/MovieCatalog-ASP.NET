﻿namespace MovieCatalog.Data.Models.ManyToMany
{
    public class MovieLanguage
    {
        public int MovieId { get; set; }

        public Movie Movie { get; set; }

        public int LanguageId { get; set; }

        public Language Language { get; set; }
    }
}
