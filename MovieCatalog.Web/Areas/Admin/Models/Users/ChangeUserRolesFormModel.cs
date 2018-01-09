namespace MovieCatalog.Web.Areas.Admin.Models.Users
{
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class ChangeUserRolesFormModel : PaginationBaseModel
    {
        public string Username { get; set; }

        public IEnumerable<string> AvailableRoles { get; set; } = new List<string>();

        public IEnumerable<SelectListItem> AvailableRolesListItem { get; set; }

        public IEnumerable<string> SelectedRoles { get; set; } = new List<string>();

        public IEnumerable<SelectListItem> SelectedRolesListItem { get; set; }

        public IEnumerable<string> UserRoles { get; set; }
    }
}
