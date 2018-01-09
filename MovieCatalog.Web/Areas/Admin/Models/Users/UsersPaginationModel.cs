namespace MovieCatalog.Web.Areas.Admin.Models.Users
{
    using System.Collections.Generic;
    using MovieCatalog.Services.Data.Models.Users;

    public class UsersPaginationModel : PaginationBaseModel
    {
        public int TotalPages { get; set; }

        public IEnumerable<UserBaseServiceModel> Users { get; set; }

        public int PreviousPage
            => this.CurrentPage == 1 ? 1 : this.CurrentPage - 1;

        public int NextPage
            => this.CurrentPage == this.TotalPages ? this.TotalPages : this.CurrentPage + 1;
    }
}
