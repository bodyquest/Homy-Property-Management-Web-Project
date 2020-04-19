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
    public class RequestsController : BaseController
    {
        private readonly UserManager<User> userManager;
        private readonly IRequestService requestService;
        private readonly IListingService listingService;

        public RequestsController(
            UserManager<User> userManager,
            IRequestService requestService,
            IListingService listingService)
        {
            this.userManager = userManager;
            this.requestService = requestService;
            this.listingService = listingService;
        }

        [ActionName("RequestForm")]
        public async Task<IActionResult> RequestFormAsync(string id)
        {
            if (!this.User.Identity.IsAuthenticated)
            {
                return this.RedirectToAction("Login", "Account", new { area = "Identity" })
                    .WithDanger(string.Empty, YouMustBeLoggedIn);
            }

            var user = await this.userManager.GetUserAsync(this.User);
            var userId = user?.Id;

            var property = await this.listingService.GetDetailsAsync(id);

            var viewModel = new RequestFormInputModel
            {
                PropertyDetails = property,
                User = user,
            };

            return this.View(viewModel);
        }

        [HttpPost]
        [ActionName("RequestForm")]
        public async Task<IActionResult> RequestFormAsync(string id, string about, string phoneNumber, string message, IFormFile document)
        {
            if (!this.User.Identity.IsAuthenticated)
            {
                return this.RedirectToAction("Login", "Account", new { area = "Identity" })
                    .WithDanger(string.Empty, YouMustBeLoggedIn);
            }

            var userId = this.userManager.GetUserId(this.User);
            var user = await this.userManager.FindByIdAsync(userId);
            var homeFromDb = await this.listingService.GetDetailsAsync(id);

            if (!this.ModelState.IsValid)
            {
                var viewModel = new RequestFormInputModel
                {
                    PropertyDetails = homeFromDb,
                    User = user,
                };

                return this.View(viewModel);
            }

            if (document.Length > RequestDocumentMaxSize)
            {
                this.TempData.AddErrorMessage(FileTooLarge);
                var viewModel = new RequestFormInputModel
                {
                    PropertyDetails = homeFromDb,
                    User = user,
                };

                return this.View(viewModel);
            }

            if (
                (string.IsNullOrWhiteSpace(about) && string.IsNullOrWhiteSpace(user.About))
                || (string.IsNullOrWhiteSpace(phoneNumber) && string.IsNullOrWhiteSpace(user.PhoneNumber))
                || string.IsNullOrWhiteSpace(message))
            {
                this.TempData.AddErrorMessage(EmptyFields);
                var viewModel = new RequestFormInputModel
                {
                    PropertyDetails = homeFromDb,
                    User = user,
                };

                return this.View(viewModel);
            }

            if (!string.IsNullOrWhiteSpace(about))
            {
                user.About = about;
                await this.userManager.UpdateAsync(user);
            }

            if (!string.IsNullOrWhiteSpace(phoneNumber))
            {
                user.PhoneNumber = phoneNumber;
                await this.userManager.UpdateAsync(user);
            }

            var fileContents = await document.ToByteArrayAsync();

            var modelForDb = new RequestCreateServiceModel
            {
                Date = DateTime.UtcNow,
                Type = (RequestType)homeFromDb.Status,
                UserId = userId,
                Message = message,
                HomeId = homeFromDb.Id,
                Document = fileContents,
            };

            bool isCreated = await this.requestService.CreateRequestAsync(modelForDb);

            if (isCreated)
            {
                return this.RedirectToAction(nameof(HomeController.Index), "Home", new { area = string.Empty })
                    .WithSuccess(string.Empty, SuccessfullySubmittedRequest);
            }

            return this.View();
        }
    }
}
