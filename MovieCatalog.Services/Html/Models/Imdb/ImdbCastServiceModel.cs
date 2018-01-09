namespace MovieCatalog.Services.Html.Models.Imdb
{
    public class ImdbCastServiceModel : ImdbArtistBaseServiceModel
    {
        public string Character { get; set; }

        public override string ToString()
            => $"{this.Name} - {this.Character}";
    }
}
