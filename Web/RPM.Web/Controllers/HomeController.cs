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

            var housesCountModel = await this.listingService.GetPropertyCountByCategoryAsync(House);
            var apartmentsCountModel = await this.listingService.GetPropertyCountByCategoryAsync(Apartment);
            var roomsCountModel = await this.listingService.GetPropertyCountByCategoryAsync(Room);

            if (housesCountModel == null)
            {
                housesCountModel = new PropertyCountServiceModel
                {
                    CategoryName = "No Houses",
                    Count = 0,
                    ExampleImage = DefaultPropertyImage,
                };
            }

            if (apartmentsCountModel == null)
            {
                apartmentsCountModel = new PropertyCountServiceModel
                {
                    CategoryName = "No Apartments",
                    Count = 0,
                    ExampleImage = DefaultPropertyImage,
                };
            }

            if (roomsCountModel == null)
            {
                roomsCountModel = new PropertyCountServiceModel
                {
                    CategoryName = "No Rooms",
                    Count = 0,
                    ExampleImage = DefaultPropertyImage,
                };
            }

            var model = new HomeIndexViewModel
            {
                PropertiesToRent = await this.listingService.GetAllByStatusAsync(ToRent),
                PropertiesToManage = await this.listingService.GetAllByStatusAsync(ToManage),
            };

            model.PropertiesByCategory.Add(housesCountModel);
            model.PropertiesByCategory.Add(apartmentsCountModel);
            model.PropertiesByCategory.Add(roomsCountModel);

            // Redundant Check ???
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
