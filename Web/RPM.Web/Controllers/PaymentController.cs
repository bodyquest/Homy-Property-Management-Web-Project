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
    using RPM.Web.Models.Home;
    using RPM.Web.Models.Payment;
    using RPM.Web.Models.Profile;
    using RPM.Web.ViewModels;
    using Stripe;
    using Stripe.Checkout;
    using static RPM.Common.GlobalConstants;

    [Authorize(Roles = TenantRole)]
    public class PaymentController : BaseController
    {
        private readonly UserManager<User> userManager;
        private readonly IPaymentService paymentService;

        public PaymentController(
            UserManager<User> userManager,
            IPaymentService paymentService)
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

        public async Task<IActionResult> Pay(string id)
        {
            var userId = this.userManager.GetUserId(this.User);
            var payment = await this.paymentService.GetPaymentDetailsAsync(id, userId);

            var viewModel = new CheckoutPaymentViewModel
            {
                Id = payment.Id,
                Date = payment.Date,
                To = payment.To,
                ToStripeAccountId = payment.ToStripeAccountId,
                Reason = payment.Reason,
                Amount = payment.Amount,
                Status = payment.Status,
                RentalAddress = payment.RentalAddress,
            };

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateSession([FromBody]string id)
        {
            var userId = this.userManager.GetUserId(this.User);
            var payment = await this.paymentService.GetPaymentDetailsAsync(id, userId);

            var successStringUrl = "https://localhost:44319/Checkout/success?session_id={CHECKOUT_SESSION_ID}";
            var cancelStringUrl = "https://localhost:44319/Checkout/cancel?";

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string>
                {
                    "card",
                },

                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        Quantity = 1,
                        Amount = (long)payment.Amount * 100,
                        Currency = CurrencyUSD,

                        Description = $"Payment Id: {payment.Id} for rental at {payment.RentalAddress}",

                        Name = $"Rent Payment for {DateTime.UtcNow.Month} | {DateTime.UtcNow.Year} for rental at {payment.RentalAddress}",
                    },
                },

                PaymentIntentData = new SessionPaymentIntentDataOptions
                {
                    ApplicationFeeAmount = (long)((payment.Amount * 0.01m) * 100),
                    CaptureMethod = "manual",

                    TransferData = new SessionPaymentIntentTransferDataOptions
                    {
                        Destination = payment.ToStripeAccountId,
                    },
                },

                SuccessUrl = successStringUrl,
                CancelUrl = cancelStringUrl,
            };

            var service = new SessionService();
            Session session = service.Create(options);

            return this.Json(session);
        }
    }
}
