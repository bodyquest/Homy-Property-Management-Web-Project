namespace RPM.Web.Areas.Management.Controllers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using RPM.Data.Models;
    using RPM.Services.Management;
    using RPM.Web.Areas.Management.Models;

    using static RPM.Common.GlobalConstants;

    [Authorize(Roles = OwnerRoleName)]
    public class DashboardController : ManagementController
    {
        private readonly IOwnerListingService listingService;
        private readonly IOwnerRequestService requestService;
        private readonly IOwnerRentalService rentalService;
        private readonly IOwnerPaymentService paymentService;
        private readonly UserManager<User> userManager;

        public DashboardController(
            IOwnerListingService listingService,
            IOwnerRequestService requestService,
            IOwnerRentalService rentalService,
            IOwnerPaymentService paymentService,
            UserManager<User> userManager)
        {
            this.listingService = listingService;
            this.requestService = requestService;
            this.rentalService = rentalService;
            this.paymentService = paymentService;
            this.userManager = userManager;
        }

        [HttpGet("/Management/Dashboard/Index")]
        public async Task<IActionResult> Index()
        {
            var userId = this.userManager.GetUserId(this.User);

            var properties = await this.listingService.GetMyPropertiesAsync(userId);
            var requests = await this.requestService.GetRequestsAsync(userId);
            var rentals = await this.rentalService.GetRentalsAsync(userId);
            var payments = await this.paymentService.AllPayments(userId);

            var viewModel = new OwnerDashboardViewModel
            {
                Properties = properties,
                Requests = requests,
                Rentals = rentals,
                Payments = payments,
            };

            return this.View(viewModel);
        }
    }
}
