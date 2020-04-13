namespace RPM.Web.Api
{
    using System.IO;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using RPM.Web.Controllers;
    using RPM.Web.Infrastructure.Extensions;
    using Stripe;

    using static RPM.Common.GlobalConstants;

    [Route("api/[controller]")]
    public class StripeEventsController : Controller
    {
        // https://dashboard.stripe.com/test/webhooks
        public const string SecretAccount = "whsec_kkeaMYEHNkXTJyAW48syt8tWSPymAKLn";
        public const string SecretConnect = "whsec_AZBLAbu7yiGC1urpKV5oatojyPPHs9CQ";

        [HttpPost]
        public async Task<IActionResult> IndexAsync()
        {
            var json = await new StreamReader(this.HttpContext.Request.Body).ReadToEndAsync();

            try
            {
                var stripeEvent = EventUtility.ConstructEvent(
                    json,
                    this.Request.Headers["Stripe-Signature"],
                    SecretConnect);

                if (stripeEvent.Type == Events.CheckoutSessionCompleted)
                {
                    var session = stripeEvent.Data.Object as Stripe.Checkout.Session;

                    // Fulfill the purchase...
                    return this.Ok();
                }
                else
                {
                    return this.NoContent();
                }
            }
            catch (StripeException exception)
            {
                return this.BadRequest(exception);
            }
            catch
            {
                return this.RedirectToAction(
                    nameof(HomeController.Index),
                    "Home",
                    new { area = string.Empty })
                    .WithWarning(string.Empty, NoLuckMan);
            }
        }
    }
}
