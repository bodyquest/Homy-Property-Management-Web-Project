namespace RPM.Web.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using RPM.Data;
    using RPM.Data.Models;
    using RPM.Services.Common;
    using RPM.Services.Common.Models.Home;
    using RPM.Web.Infrastructure.Extensions;
    using RPM.Web.Models.Home;
    using RPM.Web.Models.Profile;
    using RPM.Web.ViewModels;

    using static RPM.Common.GlobalConstants;

    [Authorize(Roles = TenantRole)]
    public class ProfileController : BaseController
    {
        private readonly UserManager<User> userManager;
        private readonly IPaymentService paymentService;

        public ProfileController(
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
    }
}