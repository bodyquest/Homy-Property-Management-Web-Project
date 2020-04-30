namespace RPM.Web.Areas.Management.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Hangfire;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using RPM.Data;
    using RPM.Data.Models;
    using RPM.Data.Models.Enums;
    using RPM.Services.Common;
    using RPM.Services.Management;
    using RPM.Services.Management.Implementations;
    using RPM.Web.Areas.Management.Models.Payments;
    using RPM.Web.Areas.Management.Models.Requests;
    using RPM.Web.Areas.Management.Models.TransactionRequests;
    using RPM.Web.Infrastructure.Extensions;
    using Stripe;
    using Stripe.Checkout;

    using static RPM.Common.GlobalConstants;

    [Authorize(Roles = "Owner, Tenant")]
    public class PaymentsController : ManagementController
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<User> userManager;
        private readonly IOwnerPaymentService paymentService;
        private readonly IPaymentCommonService paymentCommonService;

        public PaymentsController(
            ApplicationDbContext context,
            UserManager<User> userManager,
            IOwnerPaymentService paymentService,
            IPaymentCommonService paymentCommonService)
        {
            this.context = context;
            this.userManager = userManager;
            this.paymentService = paymentService;
            this.paymentCommonService = paymentCommonService;
        }

        public async Task<IActionResult> Index()
        {
            var userId = this.userManager.GetUserId(this.User);

            var model = await this.paymentService
                .AllPayments(userId);

            var viewModel = new OwnerAllPaymentsViewModel
            {
                Payments = model,
            };

            return this.View(viewModel);
        }

        [ActionName("session")]
        public async Task<IActionResult> CreateSession(string id)
        {
            var userId = this.userManager.GetUserId(this.User);
            var payment = await this.paymentService.GetPaymentDetailsAsync(id, userId);

            //// LOCALHOST LINKS
            // var successStringUrl = "https://localhost:44319/checkout/success?sessionId={CHECKOUT_SESSION_ID}";
            // var cancelStringUrl = "https://localhost:44319/checkout/cancel";

            // AZURE LINKS
            // LOCALHOST LINKS
            var successStringUrl = "https://homy.azurewebsites.net/checkout/success?sessionId={CHECKOUT_SESSION_ID}";
            var cancelStringUrl = "https://homy.azurewebsites.net/checkout/cancel";

            //// CONVEYOR LINKS
            // var successStringUrl = "https://rpm-web.conveyor.cloud/checkout/success?sessionId={CHECKOUT_SESSION_ID}";
            // var cancelStringUrl = "https://rpm-web.conveyor.cloud/checkout/cancel";

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

                        Name = $"Rent Payment for {DateTime.UtcNow.ToString("MMMM")}/ {DateTime.UtcNow.Year} at {payment.Address}",
                    },
                },

                PaymentIntentData = new SessionPaymentIntentDataOptions
                {
                    ApplicationFeeAmount = (long)((payment.Amount * 0.01m) * 100),
                    CaptureMethod = "automatic",
                    Description = payment.Id,

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

            // Create Checkout Session in Database to validate it the Webhook handler and in SuccessPage
            await this.paymentCommonService.CreateCheckoutSessionAsync(session.Id, payment.Id, payment.ToStripeAccountId);

            return this.Json(session);
        }
    }
}
