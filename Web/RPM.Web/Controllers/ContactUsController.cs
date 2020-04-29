namespace RPM.Web.Controllers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using RPM.Services.Messaging;
    using RPM.Web.Infrastructure.Extensions;
    using RPM.Web.Models.ContactUs;

    using static RPM.Common.GlobalConstants;

    public class ContactUsController : BaseController
    {
        private readonly IEmailSender emailSender;

        public ContactUsController(
            IEmailSender emailSender)
        {
            this.emailSender = emailSender;
        }

        public async Task<IActionResult> Index()
        {
            var viewModel = new ContactUsInputModel();
            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Index(ContactUsInputModel model)
        {
            if (this.ModelState.IsValid)
            {
                var email = model.Email;
                var name = model.Name;
                var subject = model.Subject;
                var content = model.Message;

                await this.emailSender.SendPlainEmailAsync(email, name, HomySupportEmail, subject, content, null);

                return this.RedirectToAction(
                    nameof(HomeController.Index),
                    "Home",
                    new { area = string.Empty })
                    .WithSuccess(string.Empty, MessageSentSuccessfully);
            }

            return this.View(model);
        }
    }
}
