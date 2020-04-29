namespace RPM.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using RPM.Data.Models;
    using RPM.Services.Admin;
    using RPM.Services.Admin.Models;
    using RPM.Services.Common;
    using RPM.Web.Areas.Administration.Models.Listings;
    using RPM.Web.Infrastructure.Extensions;

    using static RPM.Common.GlobalConstants;

    public class ListingsController : AdministrationController
    {
        private readonly IAdminListingService adminListingService;
        private readonly IAdminCityService adminCityService;
        private readonly ICloudImageService imageService;
        private readonly IImageDbService imageDbService;
        private readonly IAdminUserService adminUserService;
        private readonly UserManager<User> userManager;

        public ListingsController(
            IAdminListingService adminListingService,
            IAdminCityService adminCityService,
            ICloudImageService imageService,
            IImageDbService imageDbService,
            IAdminUserService adminUserService,
            UserManager<User> userManager)
        {
            this.adminListingService = adminListingService;
            this.adminCityService = adminCityService;
            this.imageService = imageService;
            this.imageDbService = imageDbService;
            this.adminUserService = adminUserService;
            this.userManager = userManager;
        }

        [TempData]
        public string StatusMessage { get; set; }

        // GET: Listings
        public async Task<IActionResult> Index()
        {
            var viewModel = await this.adminListingService.GetAllListingsAsync();

            return this.View(viewModel);
        }

        // GET: Create
        public IActionResult Create()
        {

            var model = new AdminListingCreateInputModel();

            return this.View(model);
        }

        // Post: Create
        [HttpPost]
        [ActionName("Create")]
        public async Task<IActionResult> CreateAsync(AdminListingCreateInputModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            var imgResult = await this.imageService
                .UploadImageAsync(model.Image);

            string imgUrl = imgResult.SecureUri.AbsoluteUri;
            string imgPubId = imgResult.PublicId;

            var imageToWrite = new CloudImage
            {
                PictureUrl = imgUrl,
                PicturePublicId = imgPubId,
            };

            var homeCreateServiceModel = new AdminHomeCreateServiceModel
            {
                Name = model.Name,
                Description = model.Description,
                Address = model.Address,
                Price = model.Price,
                City = model.City,
                Category = model.Category,
                Status = model.Status,
                Owner = model.Owner,
                Image = imageToWrite,
            };

            bool isCreated = await this.adminListingService.CreateListingAsync(homeCreateServiceModel);

            if (!isCreated)
            {
                return this.RedirectToAction("Index", "Dashboard", new { area = AdminArea }).WithWarning(string.Empty, CouldNotCreateRecord);
            }

            var owner = await this.adminUserService.GetUserByUsername(model.Owner);

            if (!await this.userManager.IsInRoleAsync(owner, OwnerRoleName))
            {
                await this.userManager.AddToRoleAsync(owner, OwnerRoleName);
            }

            await this.imageDbService.WriteToDatabasebAsync(imgUrl, imgPubId);

            return this.RedirectToAction("Index", "Dashboard", new { area = AdminArea })
                .WithSuccess(string.Empty, RecordCreatedSuccessfully);
        }
    }
}
