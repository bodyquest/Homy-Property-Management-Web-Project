namespace RPM.Web.Areas.Management.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using RPM.Data.Models;
    using RPM.Services.Management;
    using RPM.Web.Areas.Management.Models.Requests;

    public class RequestsController : ManagementController
    {
        private readonly UserManager<User> userManager;
        private readonly IOwnerRequestService requestService;

        public RequestsController(
            UserManager<User> userManager,
            IOwnerRequestService requestService)
        {
            this.userManager = userManager;
            this.requestService = requestService;
        }

        public async Task<IActionResult> Index()
        {
            // var model = await this.requestService.GetAllRequestsAsync();
            // var viewModel = new OwnerRequestsWithDetailsViewModel{};

            return this.View();
        }

        [ActionName("Details")]
        public async Task<IActionResult> DetailsAsync(string id)
        {
            var userId = this.userManager.GetUserId(this.User);

            var requestModel = await this.requestService.GetRequestDetailsAsync(id);

            var viewModel = new RequestDetailsViewModel
            {
                Id = requestModel.Id,
                Date = requestModel.Date,
                UserFirstName = requestModel.UserFirstName,
                UserLastName = requestModel.UserLastName,
                Email = requestModel.Email,
                Phone = requestModel.Phone,
                RequestType = requestModel.RequestType,
                Message = requestModel.Message,
                About = requestModel.About,
                Document = requestModel.Document,
                HomeInfo = requestModel.HomeInfo,
            };

            return this.View(viewModel);
        }

        public async Task<IActionResult> DownloadFile(string requestId)
        {
            if (string.IsNullOrWhiteSpace(requestId))
            {
                return this.BadRequest();
            }

            var requestInfo = await this.requestService.GetRequestInfoAsync(requestId);

            var documentContent = requestInfo.Document;
            var userFirstName = requestInfo.UserFirstName;
            var userLastName = requestInfo.UserLastName;
            var requestType = requestInfo.RequestType;

            if (documentContent == null)
            {
                return this.BadRequest();
            }

            return this.File(documentContent, "application/pdf", $"{userFirstName}_{userLastName}_request-{requestType}_{DateTime.UtcNow.ToString("dd-mm-yyyy")}.pdf");
        }

        [HttpPost]
        public async Task<IActionResult> UploadContract(string requestId)
        {
            return this.View();
        }
    }
}
