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
    using RPM.Web.Models.Payment;
    using RPM.Web.Models.Profile;
    using RPM.Web.ViewModels;
    using Stripe;
    using Stripe.Checkout;

    using static RPM.Common.GlobalConstants;

    [Authorize(Roles = TenantRole)]
    public class CheckoutController : BaseController
    {
        private readonly UserManager<User> userManager;
        private readonly IPaymentCommonService paymentCommonService;

        public CheckoutController(
            UserManager<User> userManager,
            IPaymentCommonService paymentCommonService)
        {
            this.userManager = userManager;
            this.paymentCommonService = paymentCommonService;
        }

        public async Task<IActionResult> Success(string sessionId)
        {
            var userId = this.userManager.GetUserId(this.User);
            bool compare = await this.paymentCommonService.CompareData(sessionId);

            if (string.IsNullOrWhiteSpace(sessionId) || compare == false)
            {
                return this.RedirectToAction(nameof(ProfileController.Index), "Home", new { area = string.Empty })
                    .WithWarning(string.Empty, NiceTry);
            }

            var paymentId = await this.paymentCommonService.GetPaymentId(sessionId);

            var model = await this.paymentCommonService.GetPaymentDetailsAsync(paymentId, userId);

            var viewModel = new CheckoutPaymentViewModel
            {
                Reason = model.Reason,
                Amount = model.Amount,
                To = model.To,
                Address = model.Address,
                TransactionDate = model.TransactionDate?.ToString(DateFormatWithTime),
            };

            return this.View(viewModel);
        }
    }
}
