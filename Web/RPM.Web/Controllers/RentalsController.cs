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

    [Authorize(Roles = TenantRole)]
    public class RentalsController : BaseController
    {
        private readonly UserManager<User> userManager;
        private readonly IRentalService rentalService;
        private readonly IListingService listingService;
        private readonly IRequestService requestService;

        public RentalsController(
            UserManager<User> userManager,
            IRentalService rentalService,
            IListingService listingService,
            IRequestService requestService)
        {
            this.userManager = userManager;
            this.rentalService = rentalService;
            this.listingService = listingService;
            this.requestService = requestService;
        }

        [ActionName("Details")]
        public async Task<IActionResult> DetailsAsync(int id)
        {

            var user = await this.userManager.GetUserAsync(this.User);
            var userId = user.Id;

            var model = await this.rentalService.GetDetailsAsync(id);

            var viewModel = new CancellationRequestInputModel
            {
                RentalInfo = model,
            };

            return this.View(viewModel);
        }

        [HttpPost]
        [ActionName("Details")]
        public async Task<IActionResult> CancelRentAsync(int id, string message, IFormFile document)
        {
            if (!this.ModelState.IsValid)
            {
                return this.RedirectToAction("Index", "Profile", new { area = string.Empty })
                    .WithWarning(string.Empty, InvalidEntryData);
            }

            var user = await this.userManager.GetUserAsync(this.User);

            var homeFromDb = await this.rentalService.GetDetailsAsync(id);
            var fileContents = await document.ToByteArrayAsync();

            var modelForDb = new RequestCreateServiceModel
            {
                Date = DateTime.UtcNow,
                Type = RequestType.CancelRent,
                UserId = user.Id,
                Message = message,
                HomeId = homeFromDb.HomeId,
                Document = fileContents,
            };

            bool isCreated = await this.requestService.CreateRequestAsync(modelForDb);

            if (isCreated)
            {
                return this.RedirectToAction("Index", "Profile", new { area = string.Empty })
                    .WithSuccess(string.Empty, SuccessfullySubmittedRequest);
            }

            return this.RedirectToAction("Index", "Profile", new { area = string.Empty })
                    .WithDanger(string.Empty, FailedToSubmitRequest);
        }
    }
}
