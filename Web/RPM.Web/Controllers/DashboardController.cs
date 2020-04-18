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
    using RPM.Services.Common.Models.Home;
    using RPM.Web.Infrastructure.Extensions;
    using RPM.Web.Models.Dashboard;
    using Stripe;
    using Stripe.Checkout;
    using static RPM.Common.GlobalConstants;

    public class DashboardController : BaseController
    {
        private readonly UserManager<User> userManager;
        private readonly IListingService listingService;
        private readonly IPaymentCommonService paymentService;

        public DashboardController(
            UserManager<User> userManager,
            IListingService listingService,
            IPaymentCommonService paymentService)
        {
            this.userManager = userManager;
            this.listingService = listingService;
            this.paymentService = paymentService;
        }

        [Authorize(Roles = ManagerRoleName)]
        public async Task<IActionResult> Index()
        {
            var userId = this.userManager.GetUserId(this.User);
            var user = await this.userManager.FindByIdAsync(userId);
            var hasStripe = !string.IsNullOrWhiteSpace(user.StripeConnectedAccountId);

            var userPayments = await this.paymentService.GetManagerPaymentsListAsync(userId);
            var managedProperties = await this.listingService.GetManagedPropertiesAsync(userId);

            var viewModel = new ManagerDashboardViewModel
            {
                HasStripeAccount = hasStripe,
                ManagedProperties = managedProperties,
                Payments = userPayments,
            };

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Pay(string paymentId, string stripeToken)
        {
            var userId = this.userManager.GetUserId(this.User);
            var payment = await this.paymentService.GetPaymentDetailsAsync(paymentId, userId);

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string>
                {
                    "card",
                },

                PaymentIntentData = new SessionPaymentIntentDataOptions
                {
                    ApplicationFeeAmount = (long)((payment.Amount * 0.01m) * 100),
                    Description = $"Payment Id: {payment.Id} for rental at {payment.Address}",
                    CaptureMethod = "manual",

                    TransferData = new SessionPaymentIntentTransferDataOptions
                    {
                        Amount = (long)payment.Amount * 100,
                        Destination = payment.ToStripeAccountId,
                    },
                },

                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        Name = "Monthly Rent",
                        Description = $"Payment Id: {payment.Id} for rental at {payment.Address}",
                        Amount = (long)payment.Amount * 100,
                        Currency = CurrencyUSD,
                    },
                },
                SuccessUrl = "https://localhost:44319/Management/Stripe/Checkout/success?session_id={CHECKOUT_SESSION_ID}",
                CancelUrl = "https://localhost:44319/Management/Stripe/Checkout/cancel",
            };

            var service = new SessionService();
            Session session = service.Create(options);

            return this.RedirectToAction("Success", new { session_id = session.Id });
        }
    }
}
