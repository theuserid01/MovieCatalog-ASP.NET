namespace MovieCatalog.Services.Html.Models.RottenTomatoes.JsonDtos
{
    using System.Collections.Generic;

    public class RottenTomatoesSearchMovieTitleJsonDtoModel
    {
        public IEnumerable<RottenTomatoesBaseJsonDtoModel> Movies { get; set; }
    }
}
