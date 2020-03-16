namespace RPM.Web.Controllers
{
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc; 
    using RPM.Data;
    using RPM.Services.Common;
    using RPM.Services.Common.Models.Home;
    using RPM.Web.Infrastructure.Extensions;
    using RPM.Web.Models.Home;
    using RPM.Web.ViewModels;

    using static RPM.Common.GlobalConstants;

    public class HomeController : BaseController
    {
        private readonly ApplicationDbContext context;
        private readonly IListingService listingService;

        public HomeController(ApplicationDbContext context, IListingService listingService)
        {
            this.context = context;
            this.listingService = listingService;
        }

        public async Task<IActionResult> Index()
        {
            if (this.User.Identity.IsAuthenticated && this.User.IsInRole(AdministratorRoleName))
            {
                return this.RedirectToAction("Index", "Dashboard", new { area = AdminArea });
            }

            var model = new HomeIndexViewModel
            {
                Properties = await this.listingService.GetPropertiesAsync(),
            };

            if (model == null)
            {
                return this.RedirectToAction(nameof(HomeController.Index), "Home", new { area = string.Empty }).WithWarning(string.Empty, CouldNotFind);
            }

            return this.View(model);
         }

        public async Task<IActionResult> Search(SearchFormModel model)
        {
            var viewModel = new SearchViewModel
            {
                SearchText = model.SearchText,
            };

            var result = await this.listingService.FindAsync(model.SearchText);

            if (result == null)
            {
                return this.RedirectToAction(nameof(HomeController.Index), "Home", new { area = string.Empty }).WithWarning(string.Empty, CouldNotFind);
            }

            viewModel.Properties = result;

            return this.View(viewModel);
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
