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
    using RPM.Services.Management;
    using RPM.Services.Management.Implementations;
    using RPM.Web.Areas.Management.Models.Payments;
    using RPM.Web.Areas.Management.Models.Requests;
    using RPM.Web.Areas.Management.Models.TransactionRequests;
    using RPM.Web.Infrastructure.Extensions;
    using Stripe;
    using Stripe.Checkout;
    using static RPM.Common.GlobalConstants;

    public class PaymentsController : ManagementController
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<User> userManager;
        private readonly IPaymentService paymentService;

        public PaymentsController(
            ApplicationDbContext context,
            UserManager<User> userManager,
            IPaymentService paymentService)
        {
            this.context = context;
            this.userManager = userManager;
            this.paymentService = paymentService;
        }

        [Authorize(Roles = OwnerRoleName)]
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

        [Authorize(Roles = TenantRole)]
        [ActionName("session")]
        public async Task<IActionResult> CreateSession(string id)
        {
            //var id = model.Id;
            //var id = "68f51bff-5709-4337-a74b-d0ecb53f33ec";
            var userId = this.userManager.GetUserId(this.User);
            var payment = await this.paymentService.GetPaymentDetailsAsync(id, userId);

            var successStringUrl = "https://localhost:44319/Checkout/success?session_id={CHECKOUT_SESSION_ID}";
            var cancelStringUrl = "https://localhost:44319/Checkout/cancel";

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

                        Name = $"Rent Payment for {DateTime.UtcNow.ToString("MMMM")}/ {DateTime.UtcNow.Year} at {payment.RentalAddress}",
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
