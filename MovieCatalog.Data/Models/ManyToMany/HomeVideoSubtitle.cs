namespace MovieCatalog.Data.Models.ManyToMany
{
    public class HomeVideoSubtitle
    {
        public int HomeVideoId { get; set; }

        public HomeVideo HomeVideo { get; set; }

        public int SubtitleId { get; set; }

        public Language Subtitle { get; set; }
    }
}
