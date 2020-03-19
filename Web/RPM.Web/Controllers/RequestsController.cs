namespace RPM.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using RPM.Data.Models;
    using RPM.Services.Common;
    using RPM.Services.Common.Models.Request;
    using RPM.Web.Infrastructure.Extensions;
    using RPM.Web.Models.Requests;
    using static RPM.Common.GlobalConstants;

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
        [ActionName("Submit")]
        public async Task<IActionResult> SubmitAsync(RequestFormInputModel model)
        {
            if (!this.User.Identity.IsAuthenticated)
            {
                return this.RedirectToAction("Login", "Account", new { area = "Identity" })
                    .WithDanger(string.Empty, YouMustBeLoggedIn);
            }

            var modelForDb = new RequestCreateServiceModel
            {

            };

            bool isCreated = await this.requestService.CreateRequestAsync(modelForDb);


            return this.View();
        }
    }
}
