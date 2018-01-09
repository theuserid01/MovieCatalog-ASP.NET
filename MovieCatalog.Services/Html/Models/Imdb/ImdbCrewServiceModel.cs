namespace MovieCatalog.Services.Html.Models.Imdb
{
    using MovieCatalog.Data.Models.Enums;

    public class ImdbCrewServiceModel : ImdbArtistBaseServiceModel
    {
        public CrewRole Role { get; set; }

        public override string ToString()
            => $"{this.Role}: {this.Name}";
    }
}
