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

    [Authorize(Roles = ManagerRoleName)]
    public class ManagedHomesController : BaseController
    {
        private readonly UserManager<User> userManager;
        private readonly IRentalService rentalService;
        private readonly IListingService listingService;
        private readonly IRequestService requestService;

        public ManagedHomesController(
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
        public async Task<IActionResult> DetailsAsync(string id)
        {
            var homeModel = await this.listingService.GetManagedDetailsAsync(id);

            var viewModel = new ManageCancellationRequestInputModel
            {
                HomeInfo = homeModel,
            };

            return this.View(viewModel);
        }

        [HttpPost]
        [ActionName("HomeDetails")]
        public async Task<IActionResult> CancelManageAsync(string id, string message, IFormFile document)
        {

            if (!this.ModelState.IsValid)
            {
                return this.RedirectToAction("Index", "Dashboard", new { area = string.Empty })
                    .WithWarning(string.Empty, InvalidEntryData);
            }

            var user = await this.userManager.GetUserAsync(this.User);

            var homeFromDb = await this.listingService.GetDetailsAsync(id);
            var fileContents = await document.ToByteArrayAsync();

            var modelForDb = new RequestCreateServiceModel
            {
                Date = DateTime.UtcNow,
                Type = RequestType.CancelManage,
                UserId = user.Id,
                Message = message,
                HomeId = homeFromDb.Id,
                Document = fileContents,
            };

            bool isCreated = await this.requestService.CreateRequestAsync(modelForDb);

            if (isCreated)
            {
                return this.RedirectToAction("Index", "Dashboard", new { area = string.Empty })
                    .WithSuccess(string.Empty, SuccessfullySubmittedRequest);
            }

            return this.RedirectToAction("Index", "Dashboard", new { area = string.Empty })
                    .WithDanger(string.Empty, FailedToSubmitRequest);
        }
    }
}
