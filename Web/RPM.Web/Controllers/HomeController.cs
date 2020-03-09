namespace RPM.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using System.Diagnostics;

    using RPM.Web.ViewModels;

    using static RPM.Common.GlobalConstants;

    public class HomeController : BaseController
    {
        public IActionResult Index()
        {
            if (this.User.Identity.IsAuthenticated && this.User.IsInRole(AdministratorRoleName))
            {
                return this.RedirectToAction("Index", "Dashboard", new { area = AdminArea });
            }

            return this.View();
        }

        public IActionResult Privacy()
        {
            return this.View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return this.View(
                new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
        }
    }
}
