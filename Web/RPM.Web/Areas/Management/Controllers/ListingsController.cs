namespace RPM.Web.Areas.Management.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using RPM.Data.Models;
    using RPM.Services.Admin;
    using RPM.Services.Common;
    using RPM.Services.Management;
    using RPM.Services.Management.Models;
    using RPM.Web.Areas.Management.Models.Listings;

    using RPM.Web.Infrastructure.Extensions;
    using static RPM.Common.GlobalConstants;

    public class ListingsController : ManagementController
    {
        private readonly IOwnerListingService listingService;
        private readonly ICityService cityService;
        private readonly ICountryService countryService;
        private readonly ICloudImageService imageService;
        private readonly IImageDbService imageDbService;
        private readonly UserManager<User> userManager;

        public ListingsController(
            IOwnerListingService listingService,
            ICityService cityService,
            ICountryService countryService,
            ICloudImageService imageService,
            IImageDbService imageDbService,
            UserManager<User> userManager)
        {
            this.listingService = listingService;
            this.cityService = cityService;
            this.countryService = countryService;
            this.imageService = imageService;
            this.imageDbService = imageDbService;
            this.userManager = userManager;
        }

        [Authorize(Roles = OwnerRoleName)]
        public async Task<IActionResult> Index()
        {
            var userId = this.userManager.GetUserId(this.User);
            var model = await this.listingService.GetMyPropertiesWithDetailsAsync(userId);

            var viewModel = new OwnerAllPropertiesWithDetailsViewModel
            {
                MyProperties = model,
            };

            return this.View(viewModel);
        }

        [Authorize(Roles = OwnerRoleName)]
        [ActionName("All")]
        public async Task<IActionResult> AllAsync(string id)
        {
            return this.View();
        }

        [Authorize(Roles = OwnerRoleName)]
        [ActionName("Create")]
        public async Task<IActionResult> Create()
        {
            var cities = await this.cityService.AllCitiesAsync();
            var countries = await this.countryService.AllCountriesAsync();

            var user = await this.userManager.GetUserAsync(this.User);
            var userStripeAccount = user.StripeConnectedAccountId;
            bool hasStripe = true;

            if (string.IsNullOrWhiteSpace(userStripeAccount))
            {
                hasStripe = false;
            }

            var viewModel = new OwnerListingCreateInputModel
            {
                UserHasStripeAccount = hasStripe,
                Cities = cities,
                Countries = countries,
            };

            return this.View(viewModel);
        }

        [Authorize(Roles = OwnerRoleName)]
        [HttpPost]
        [ActionName("Create")]
        public async Task<IActionResult> CreatePostAsync(OwnerListingCreateInputModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            var userId = this.userManager.GetUserId(this.User);
            var user = await this.userManager.FindByIdAsync(userId);

            var imgResult = await this.imageService
                .UploadImageAsync(model.Image);

            string imgUrl = imgResult.SecureUri.AbsoluteUri;
            string imgPubId = imgResult.PublicId;

            var imageToWrite = new CloudImage
            {
                PictureUrl = imgUrl,
                PicturePublicId = imgPubId,
            };

            var homeCreateServiceModel = new OwnerCreateListingServiceModel
            {
                Name = model.Name,
                Description = model.Description,
                Address = model.Address,
                Price = model.Price,
                CityId = model.CityId,
                Category = model.Category,
                Status = model.Status,
                Owner = user,
                Image = imageToWrite,
            };

            bool isCreated = await this.listingService.CreateListingAsync(homeCreateServiceModel);

            if (!isCreated)
            {
                return this.RedirectToAction("Index", "Dashboard", new { area = ManagementArea })
                    .WithWarning(string.Empty, CouldNotCreateRecord);
            }

            await this.imageDbService.WriteToDatabasebAsync(imgUrl, imgPubId);

            return this.RedirectToAction("Index", "Dashboard", new { area = ManagementArea })
                .WithSuccess(string.Empty, RecordCreatedSuccessfully);
        }

        [Authorize(Roles = OwnerRoleName)]
        [ActionName("Edit")]
        public async Task<IActionResult> Edit(string id)
        {
            var user = await this.userManager.GetUserAsync(this.User);

            var model = await this.listingService.GetEditModelAsync(user.Id, id);

            var viewModel = new OwnerListingEditInputModel
            {
                Id = model.Id,
                Name = model.Name,
                Description = model.Description,
                Address = model.Address,
                City = model.City,
                Country = model.Country,
                Price = model.Price,
                Status = model.Status,
                Category = model.Category,
                ImageFromDb = model.Image,
                RentalInfo = model.RentalInfo,
            };

            return this.View(viewModel);
        }

        [Authorize(Roles = OwnerRoleName)]
        [HttpPost]
        [ActionName("Edit")]
        public async Task<IActionResult> EditPostAsync(OwnerListingEditInputModel model)
        {

            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            var userId = this.userManager.GetUserId(this.User);
            var user = await this.userManager.FindByIdAsync(userId);

            var homeEditModel = new OwnerEditListingServiceModel();
            string imgUrl = string.Empty;
            string imgPubId = string.Empty;

            var files = this.HttpContext.Request.Form.Files;
            if (model.Image != null)
            {
                var imgResult = await this.imageService
                .UploadImageAsync(model.Image);

                imgUrl = imgResult.SecureUri.AbsoluteUri;
                imgPubId = imgResult.PublicId;

                var imageToWrite = new CloudImage
                {
                    PictureUrl = imgUrl,
                    PicturePublicId = imgPubId,
                };

                homeEditModel = new OwnerEditListingServiceModel
                {
                    Id = model.Id,
                    Name = model.Name,
                    Description = model.Description,
                    Price = model.Price,
                    Category = model.Category,
                    Status = model.Status,
                    Image = imageToWrite,
                };
            }
            else
            {
                homeEditModel = new OwnerEditListingServiceModel
                {
                    Id = model.Id,
                    Name = model.Name,
                    Description = model.Description,
                    Price = model.Price,
                    Category = model.Category,
                    Status = model.Status,
                };
            }

            bool isEdited = await this.listingService.EditListingAsync(homeEditModel);

            if (!isEdited)
            {
                return this.RedirectToAction("Details", "Listings", new { id = model.Id, Area = ManagementArea })
                    .WithWarning(string.Empty, CouldNotUpdateRecord);
            }

            await this.imageDbService.WriteToDatabasebAsync(imgUrl, imgPubId);

            return this.RedirectToAction("Details", "Listings", new { id = model.Id, area = ManagementArea })
                .WithSuccess(string.Empty, RecordUpdatedSuccessfully);
        }

        /*[Authorize(Roles = OwnerRoleName)]
        [ActionName("Delete")]
        [HttpPost]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            var user = await this.userManager.GetUserAsync(this.User);
            var isDeletable = await this.listingService.IsHomeDeletable(id);
            if(!isDeletable)
            {
                return this.RedirectToAction("Index", "Dashboard", new { area = ManagementArea })
                    .WithWarning(string.Empty, NotAllowedToRemove);
            }

            var result = await this.listingService.DeleteAsync(user.Id, id);

            var viewModel = new OwnerListingDeleteInputModel
            {
                Id = model.Id,
                Name = model.Name,
                Description = model.Description,
                Address = model.Address,
                City = model.City,
                Country = model.Country,
                Price = model.Price,
                Status = model.Status,
                Category = model.Category,
                ImageFromDb = model.Image,
                RentalInfo = model.RentalInfo,
            };

            return this.RedirectToAction("Index", "Dashboard", new { area = ManagementArea })
                    .WithSuccess(string.Empty, RemovedSuccessfully);
        }*/

        [Authorize(Roles = OwnerRoleName)]
        [ActionName("Details")]
        public async Task<IActionResult> DetailsAsync(string id)
        {
            var userId = this.userManager.GetUserId(this.User);
            var model = await this.listingService.GetDetailsAsync(userId, id);

            return this.View(model);
        }

        [Authorize(Roles = "Owner, Viewer")]
        [ActionName("GetCity")]
        public async Task<IActionResult> GetCityAsync(int id)
        {
            var model = await this.cityService.AllCitiesByCountryAsync(id);

            return this.Json(new SelectList(model, "Id", "Name"));
        }
    }
}
