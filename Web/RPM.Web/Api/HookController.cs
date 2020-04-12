namespace RPM.Web.Api
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using RPM.Data;
    using RPM.Data.Models;
    using RPM.Services.Common;
    using RPM.Web.Api.Models;
    using Stripe;
    using Stripe.Checkout;

    using static RPM.Common.GlobalConstants;

    [Route("api/[controller]")]
    [ApiController]
    public class HookController : ControllerBase
    {
        // https://dashboard.stripe.com/test/webhooks
        public const string Secret = "whsec_kkeaMYEHNkXTJyAW48syt8tWSPymAKLn";

        [HttpPost]
        [Route("handle")]
        public async Task<IActionResult> HandleAsync()
        {
            var json = await new StreamReader(this.HttpContext.Request.Body).ReadToEndAsync();

            try
            {
                var stripeEvent = EventUtility.ConstructEvent(
                    json,
                    this.Request.Headers["Stripe-Signature"],
                    Secret);

                // Handle the checkout.session.completed event
                if (stripeEvent.Type == Events.CheckoutSessionCompleted)
                {
                    var session = stripeEvent.Data.Object as Stripe.Checkout.Session;

                    // Fulfill the purchase...
                    this.HandleCheckoutSession(session);
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

        private void HandleCheckoutSession(Session session)
        {
            // mark payment as completed in the DB

            throw new NotImplementedException();
        }
    }
}
