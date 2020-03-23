namespace RPM.Web.Areas.Management.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using RPM.Common.Extensions;
    using RPM.Data.Models;
    using RPM.Services.Management;
    using RPM.Web.Areas.Management.Models.Requests;

    using RPM.Web.Infrastructure.Extensions;
    using static RPM.Common.GlobalConstants;

    public class RequestsController : ManagementController
    {
        private readonly UserManager<User> userManager;
        private readonly IOwnerRequestService requestService;
        private readonly IOwnerRentalService rentalService;

        public RequestsController(
            UserManager<User> userManager,
            IOwnerRequestService requestService,
            IOwnerRentalService rentalService)
        {
            this.userManager = userManager;
            this.requestService = requestService;
            this.rentalService = rentalService;
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
        [ActionName("Approve")]
        public async Task<IActionResult> ApproveAsync(string id, IFormFile contract)
        {
            var request = await this.requestService.GetRequestInfoAsync(id);
            var requestType = request.RequestType;
            var userId = request.UserId;
            var user = this.userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return this.RedirectToAction("Index", "Dashboard", new { area = "Management" })
                    .WithDanger(string.Empty, UserNotFound);
            }

            if (contract.Length > ContractDocumentMaxSize)
            {
                this.TempData.AddErrorMessage(FileTooLarge);
                return await this.DetailsAsync(id);
            }

            var fileContents = await contract.ToByteArrayAsync();

            if (requestType == ToRent)
            {
                var isRentalCreated = await this.rentalService.StartRent(id, fileContents);
                if (!isRentalCreated)
                {
                    return this.BadRequest();
                }

                return this.RedirectToAction("Index", "Dashboard", new { area = "Management" })
                    .WithSuccess(string.Empty, RentCreatedSuccessfully);
            }
            else if (requestType == ToManage)
            {

            }
            else if (requestType == CancelRent)
            {

            }
            else if (requestType == CancelManage)
            {

            }

            return this.BadRequest();
        }
    }
}
