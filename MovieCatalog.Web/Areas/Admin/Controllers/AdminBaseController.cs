namespace MovieCatalog.Web.Areas.Admin.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using MovieCatalog.Common;

    [Area(WebConstants.AdminArea)]
    [Authorize(Roles = GlobalConstants.AdministratorRole)]
    public abstract class AdminBaseController : Controller
    {
    }
}
