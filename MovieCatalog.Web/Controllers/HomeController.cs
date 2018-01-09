namespace MovieCatalog.Web.Controllers
{
    using System.Diagnostics;
    using Microsoft.AspNetCore.Mvc;
    using MovieCatalog.Web.Models;

    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return RedirectToAction(nameof(HomeController.Index), "Home", new { area = "Movies" });
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
