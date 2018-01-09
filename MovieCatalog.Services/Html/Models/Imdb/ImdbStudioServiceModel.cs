namespace MovieCatalog.Services.Html.Models.Imdb
{
    using MovieCatalog.Data.Models.Enums;

    public class ImdbStudioServiceModel
    {
        public string Name { get; set; }

        public StudioRole Role { get; set; }

        public override string ToString()
            => $"{this.Role}: {this.Name}";
    }
}
