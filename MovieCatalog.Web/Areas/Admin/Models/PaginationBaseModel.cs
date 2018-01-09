namespace MovieCatalog.Web.Areas.Admin.Models
{
    public abstract class PaginationBaseModel
    {
        private int currentPage;

        public int CurrentPage
        {
            get => this.currentPage;

            set
            {
                if (value < 1)
                {
                    value = 1;
                }

                this.currentPage = value;
            }
        }

        public string Search { get; set; }

        public string PageAndSearchQuery => this.FormatPageQuery() + this.FormatSearchQuery();

        private string FormatPageQuery()
        {
            return $"?page={this.CurrentPage}";
        }

        private string FormatSearchQuery()
        {
            if (string.IsNullOrWhiteSpace(this.Search))
            {
                return string.Empty;
            }

            return $"&search={this.Search}";
        }
    }
}
