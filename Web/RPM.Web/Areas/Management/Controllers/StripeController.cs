namespace RPM.Web.Areas.Management.Controllers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using RPM.Data.Models;
    using RPM.Web.Infrastructure.Extensions;
    using Stripe;

    using static RPM.Common.GlobalConstants;

    [Authorize(ManagerRoleName)]
    public class StripeController : ManagementController
    {
        private readonly UserManager<User> userManager;

        public StripeController(UserManager<User> userManager)
        {
            this.userManager = userManager;
        }

        public async Task<IActionResult> StripeCallback(string code)
        {
            var userId = this.userManager.GetUserId(this.User);
            var user = await this.userManager.FindByIdAsync(userId);

            var options = new OAuthTokenCreateOptions()
            {
                GrantType = "authorization_code",
                Code = code,
            };

            var service = new OAuthTokenService();
            var response = service.Create(options);

            user.StripeConnectedAccountId = response.StripeUserId;
            user.StripePublishableKey = response.StripePublishableKey;
            user.StripeRefreshToken = response.RefreshToken;
            // user.StripeAccessToken = response.AccessToken;

            await this.userManager.UpdateAsync(user);

            return this.RedirectToAction("Create", "Listings", new { area = ManagementArea })
                .WithSuccess(string.Empty, SuccessfullyRegistered);
        }
    }
}
