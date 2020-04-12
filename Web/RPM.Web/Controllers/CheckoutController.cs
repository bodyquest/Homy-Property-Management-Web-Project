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
    public class CheckoutController : BaseController
    {
        private readonly UserManager<User> userManager;
        private readonly IPaymentCommonService paymentService;

        public CheckoutController(
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

        public async Task<IActionResult> Success(string sessionId)
        {
            var userId = this.userManager.GetUserId(this.User);

            StripeConfiguration.ApiKey = HomyTestSecretKey;
            var service = new SessionService();
            Session checkoutSession = service.Get(sessionId);

            var paymentId = checkoutSession
                .PaymentIntent
                .TransferData
                .Destination.ToString();

            var intentId = checkoutSession.SetupIntentId;
            var intentService = new SetupIntentService();
            var intent = intentService.Get(intentId);
            var result = intent.PaymentMethod.StripeResponse.IdempotencyKey;

            var payment = await this.paymentService.GetPaymentDetailsAsync(paymentId, userId);
            var paymentStatus = PaymentStatus.Complete;
            var transactionDate = DateTime.UtcNow;
            // var result = await this.paymentService.EditPaymentStatusAsync(paymentId, userId, status, transactionDate);

            return this.View();
        }
    }
}
