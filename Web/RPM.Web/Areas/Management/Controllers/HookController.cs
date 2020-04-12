namespace RPM.Web.Areas.Management.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using Hangfire;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using RPM.Data;
    using RPM.Data.Models;
    using RPM.Data.Models.Enums;
    using RPM.Services.Management;
    using RPM.Services.Management.Implementations;
    using RPM.Web.Areas.Management.Models.Payments;
    using RPM.Web.Areas.Management.Models.Requests;
    using RPM.Web.Areas.Management.Models.TransactionRequests;
    using RPM.Web.Infrastructure.Extensions;
    using Stripe;
    using Stripe.Checkout;
    using static RPM.Common.GlobalConstants;

    public class HookController : ManagementController
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<User> userManager;
        private readonly IPaymentService paymentService;

        public HookController(
            ApplicationDbContext context,
            UserManager<User> userManager,
            IPaymentService paymentService)
        {
            this.context = context;
            this.userManager = userManager;
            this.paymentService = paymentService;
        }

        // https://dashboard.stripe.com/test/webhooks
        public const string Secret = "whsec_kkeaMYEHNkXTJyAW48syt8tWSPymAKLn";

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
