namespace RPM.Web.Areas.Management.Controllers
{
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using RPM.Data;
    using RPM.Data.Models;
    using Stripe;
    using Stripe.Checkout;
    using static RPM.Common.GlobalConstants;

    // TO REMOVE CONTROLLER
    public class HookController : ManagementController
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<User> userManager;

        public HookController(
            ApplicationDbContext context,
            UserManager<User> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        // https://dashboard.stripe.com/test/webhooks
        public const string Secret = "whsec_kkeaMYEHNkXTJyAW48syt8tWSPymAKLn";
        public const string SecretConnect = "whsec_AZBLAbu7yiGC1urpKV5oatojyPPHs9CQ";

        [HttpPost]
        [ActionName("handle")]
        public async Task<IActionResult> HandleAsync()
        {
            var json = await new StreamReader(this.HttpContext.Request.Body).ReadToEndAsync();

            try
            {
                var stripeEvent = EventUtility.ConstructEvent(
                    json,
                    this.Request.Headers["Stripe-Signature"],
                    SecretConnect);

                // Handle the checkout.session.completed event
                if (stripeEvent.Type == Events.CheckoutSessionCompleted)
                {
                    var session = stripeEvent.Data.Object as Stripe.Checkout.Session;

                    return this.Ok();
                }
                else
                {
                    return this.Ok();
                }
            }
            catch (StripeException e)
            {
                return this.BadRequest();
            }
        }
    }
}
