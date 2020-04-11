namespace RPM.Web.Api
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using RPM.Data;
    using RPM.Data.Models;
    using RPM.Services.Common;
    using RPM.Web.Controllers;
    using Stripe;
    using Stripe.Checkout;

    using static RPM.Common.GlobalConstants;

    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : BaseController
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<User> userManager;
        private readonly IPaymentCommonService paymentService;

        public PaymentController(
            ApplicationDbContext context,
            UserManager<User> userManager,
            IPaymentCommonService paymentService)
        {
            this.context = context;
            this.userManager = userManager;
            this.paymentService = paymentService;
        }

        public class ParamsModel
        {
            public string Id { get; set; }
        }

        // Create PaymentSession
        //[HttpPost]
        //[Route("session")]
        //public async Task<Session> CreateSession(string id)
        //{
        //    var userId = this.userManager.GetUserId(this.User);
        //    var payment = await this.paymentService.GetPaymentDetailsAsync(id, userId);

        //    var successStringUrl = "https://localhost:44319/Checkout/success?session_id={CHECKOUT_SESSION_ID}";
        //    var cancelStringUrl = "https://localhost:44319/Checkout/cancel";

        //    var options = new SessionCreateOptions
        //    {
        //        PaymentMethodTypes = new List<string>
        //        {
        //            "card",
        //        },

        //        LineItems = new List<SessionLineItemOptions>
        //        {
        //            new SessionLineItemOptions
        //            {
        //                Quantity = 1,
        //                Amount = (long)payment.Amount * 100,
        //                Currency = CurrencyUSD,

        //                Description = $"Payment Id: {payment.Id} for rental at {payment.RentalAddress}",

        //                Name = $"Rent Payment for {DateTime.UtcNow.Month} | {DateTime.UtcNow.Year} for rental at {payment.RentalAddress}",
        //            },
        //        },

        //        PaymentIntentData = new SessionPaymentIntentDataOptions
        //        {
        //            ApplicationFeeAmount = (long)((payment.Amount * 0.01m) * 100),
        //            CaptureMethod = "manual",

        //            TransferData = new SessionPaymentIntentTransferDataOptions
        //            {
        //                Destination = payment.ToStripeAccountId,
        //            },
        //        },

        //        SuccessUrl = successStringUrl,
        //        CancelUrl = cancelStringUrl,
        //    };

        //    var service = new SessionService();
        //    Session session = service.Create(options);

        //    return session;
        //}
    }
}
