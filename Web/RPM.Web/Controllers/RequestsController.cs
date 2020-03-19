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
    using RPM.Web.Infrastructure.Extensions;

    using static RPM.Common.GlobalConstants;

    public class RequestsController : BaseController
    {
        private readonly UserManager<User> userManager;
        private readonly IRequestService requestService;

        public RequestsController(UserManager<User> userManager, IRequestService requestService)
        {
            this.userManager = userManager;
            this.requestService = requestService;
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

            var model = await this.requestService.GetFormDataAsync(id, user);

            //var viewModel = new RequestFormViewModel
            //{

            //};

            return this.View();
        }

        [HttpPost]
        [ActionName("RequestForm")]
        public async Task<IActionResult> RequestFormAsync(/*RequestFormInputModel model*/)
        {
            if (!this.User.Identity.IsAuthenticated)
            {
                return this.RedirectToAction("Login", "Account", new { area = "Identity" })
                    .WithDanger(string.Empty, YouMustBeLoggedIn);
            }



            return this.View();
        }
    }
}
