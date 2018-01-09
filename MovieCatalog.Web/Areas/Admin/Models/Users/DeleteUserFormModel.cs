namespace MovieCatalog.Web.Areas.Admin.Models.Users
{
    public class DeleteUserFormModel : PaginationBaseModel
    {
        public string Email { get; set; }

        public string Username { get; set; }
    }
}
