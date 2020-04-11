namespace RPM.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using RPM.Data;
    using RPM.Data.Models;
    using RPM.Data.Models.Enums;
    using RPM.Services.Common;
    using RPM.Services.Common.Models.Home;
    using RPM.Web.Infrastructure.Extensions;
    using RPM.Web.Models.Home;
    using RPM.Web.Models.Profile;
    using RPM.Web.ViewModels;
    using Stripe;
    using Stripe.Checkout;
    using static RPM.Common.GlobalConstants;

    [Authorize(Roles = TenantRole)]
    public class ProfileController : BaseController
    {
        private readonly UserManager<User> userManager;
        private readonly IPaymentCommonService paymentService;

        public ProfileController(
            UserManager<User> userManager,
            IPaymentCommonService paymentService)
        {
            this.userManager = userManager;
            this.paymentService = paymentService;
        }

        public async Task<IActionResult> Index()
        {
            var userId = this.userManager.GetUserId(this.User);

            var userPayments = await this.paymentService.GetUserPaymentsListAsync(userId);

            var viewModel = new ProfileIndexViewModel
            {
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
                    Description = $"Payment Id: {payment.Id} for rental at {payment.RentalAddress}",
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
                        Description = $"Payment Id: {payment.Id} for rental at {payment.RentalAddress}",
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
