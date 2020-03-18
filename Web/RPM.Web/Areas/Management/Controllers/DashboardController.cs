namespace RPM.Web.Areas.Management.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using RPM.Services.Management;
    using RPM.Web.Areas.Management.Models;
    using static RPM.Common.GlobalConstants;

    public class DashboardController : ManagementController
    {
        private readonly IOwnerListingService listingService;
        private readonly IOwnerRequestService requestService;
        private readonly IOwnerRentalService rentalService;

        public DashboardController(
            IOwnerListingService listingService,
            IOwnerRequestService requestService,
            IOwnerRentalService rentalService)
        {
            this.listingService = listingService;
            this.requestService = requestService;
            this.rentalService = rentalService;
        }

        [HttpGet("/Management/Dashboard/Index")]
        public async Task<IActionResult> Index()
        {
            var properties = await this.listingService.GetMyPropertiesAsync();
            var requests = await this.requestService.GetRequestsAsync();
            var rentals = await this.rentalService.GetRentalsAsync();

            var viewModel = new OwnerDashboardViewModel
            {
                Properties = properties,
                Requests = requests,
                Rentals = rentals,
            };

            return this.View();
        }



    }
}
