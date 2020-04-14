namespace RPM.Web.Areas.Management.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using RPM.Data.Models;
    using RPM.Services.Management;
    using RPM.Web.Areas.Management.Models.Rentals;

    using static RPM.Common.GlobalConstants;

    [Authorize(ManagerRoleName)]
    public class RentalsController : ManagementController
    {
        private readonly UserManager<User> userManager;
        private readonly IOwnerRentalService rentalService;

        public RentalsController(
            UserManager<User> userManager,
            IOwnerRentalService rentalService)
        {
            this.userManager = userManager;
            this.rentalService = rentalService;
        }

        public async Task<IActionResult> Index()
        {
            var userId = this.userManager.GetUserId(this.User);

            var model = await this.rentalService.GetAllRentalsWithDetailsAsync(userId);
            var viewModel = new OwnerRentalsWithDetailsViewModel
            {
                Rentals = model,
            };

            return this.View(viewModel);
        }
    }
}
