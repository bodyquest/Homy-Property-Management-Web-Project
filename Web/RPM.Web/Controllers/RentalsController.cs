namespace RPM.Web.Controllers
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using RPM.Common.Extensions;
    using RPM.Data.Models;
    using RPM.Data.Models.Enums;
    using RPM.Services.Common;
    using RPM.Services.Common.Models.Request;
    using RPM.Web.Infrastructure.Extensions;
    using RPM.Web.Models.Requests;

    using static RPM.Common.GlobalConstants;

    [Authorize]
    public class RentalsController : BaseController
    {
        private readonly UserManager<User> userManager;
        private readonly IRentalService rentalService;
        private readonly IListingService listingService;

        public RentalsController(
            UserManager<User> userManager,
            IRentalService rentalService,
            IListingService listingService)
        {
            this.userManager = userManager;
            this.rentalService = rentalService;
            this.listingService = listingService;
        }

        [ActionName("Details")]
        public async Task<IActionResult> DetailsAsync(int id)
        {

            var user = await this.userManager.GetUserAsync(this.User);
            var userId = user.Id;

            var model = await this.rentalService.GetDetailsAsync(userId, id);

            var viewModel = new CancellationRequestInputModel
            {
                RentalInfo = model,
            };

            return this.View(viewModel);
        }

        [HttpPost]
        [ActionName("Details")]
        public async Task<IActionResult> RequestAsync(string id, string message, IFormFile document)
        {
            return this.View();
        }
    }
}
