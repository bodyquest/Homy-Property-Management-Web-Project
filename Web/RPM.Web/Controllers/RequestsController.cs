namespace RPM.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using RPM.Web.Infrastructure.Extensions;

    using static RPM.Common.GlobalConstants;

    public class RequestsController : BaseController
    {
        public RequestsController()
        {
        }

        [ActionName("RequestForm")]
        public async Task<IActionResult> RequestFormAsync()
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
