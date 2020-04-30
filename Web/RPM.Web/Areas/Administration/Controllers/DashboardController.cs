namespace RPM.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using RPM.Services.Admin;
    using RPM.Services.Admin.Models;
    using RPM.Services.Data;

    public class DashboardController : AdministrationController
    {
        private readonly ISettingsService settingsService;
        private readonly IAdminUserService usersService;
        private readonly IAdminListingService listingsService;
        private readonly IAdminRentalService rentalsService;

        public DashboardController(
            ISettingsService settingsService,
            IAdminUserService usersService,
            IAdminListingService listingsService,
            IAdminRentalService rentalsService)
        {
            this.settingsService = settingsService;
            this.usersService = usersService;
            this.listingsService = listingsService;
            this.rentalsService = rentalsService;
        }

        [HttpGet("/Administration/Dashboard/Index")]
        public async Task<IActionResult> Index()
        {
            var users = await this.usersService.GetUsersCount();
            var properties = await this.listingsService.GetListingsCount();
            var rentals = await this.rentalsService.GetRentalsCount();

            var viewModel = new AdminDashboardViewModel
            {
                Users = users,
                Properties = properties,
                Rentals = rentals,
            };

            return this.View(viewModel);
        }
    }
}
