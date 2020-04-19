namespace RPM.Web.Controllers
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
    using RPM.Data.Models.Enums;
    using RPM.Services.Common;
    using RPM.Services.Management;
    using RPM.Services.Management.Models;
    using RPM.Web.Areas.Management.Models.Listings;
    using RPM.Web.Infrastructure.Extensions;
    using RPM.Web.Models.Listings;

    using static RPM.Common.GlobalConstants;

    [Authorize]
    public class ListingsController : BaseController
    {
        private readonly IListingService listingService;
        private readonly IOwnerListingService ownerListingService;
        private readonly ICityService cityService;
        private readonly ICountryService countryService;
        private readonly ICloudImageService imageService;
        private readonly IImageDbService imageDbService;
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;

        public ListingsController(
            IListingService listingService,
            IOwnerListingService ownerListingService,
            ICityService cityService,
            ICountryService countryService,
            ICloudImageService imageService,
            IImageDbService imageDbService,
            UserManager<User> userManager,
            SignInManager<User> signInManager)
        {
            this.listingService = listingService;
            this.ownerListingService = ownerListingService;
            this.cityService = cityService;
            this.countryService = countryService;
            this.imageService = imageService;
            this.imageDbService = imageDbService;
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [ActionName("GetCity")]
        public async Task<IActionResult> GetCityAsync(int id)
        {
            var model = await this.cityService.AllCitiesByCountryAsync(id);

            return this.Json(new SelectList(model, "Id", "Name"));
        }

        [ActionName("Create")]
        public async Task<IActionResult> Create()
        {
            var cities = await this.cityService.AllCitiesAsync();
            var countries = await this.countryService.AllCountriesAsync();

            var userId = this.userManager.GetUserId(this.User);
            var user = await this.userManager.FindByIdAsync(userId);

            var userStripeAccount = user.StripeConnectedAccountId;
            bool? hasStripe = true;

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

            // Upload the Image
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

            // Create Listing
            bool isCreated = await this.ownerListingService.CreateListingAsync(homeCreateServiceModel);

            if (!isCreated)
            {
                return this.RedirectToAction("Index", "Dashboard", new { area = string.Empty })
                    .WithWarning(string.Empty, CouldNotCreateRecord);
            }

            // Add To Role
            await this.userManager.AddToRoleAsync(user, OwnerRoleName);

            // Referesh authorization since redirect requires the new role auth
            await this.signInManager.RefreshSignInAsync(user);

            // Write Image to DB
            await this.imageDbService.WriteToDatabasebAsync(imgUrl, imgPubId);

            return this.RedirectToAction("Index", "Dashboard", new { area = ManagementArea })
                .WithSuccess(string.Empty, RecordCreatedSuccessfully);
        }

        [ActionName("Details")]
        public async Task<IActionResult> DetailsAsync(string id)
        {
            var model = await this.listingService.GetDetailsAsync(id);

            if (model == null)
            {
                return this.NotFound();
            }

            var viewModel = new PropertyDetailsViewModel
            {
                Id = model.Id,
                Name = model.Name,
                OwnerName = model.OwnerName,
                City = model.City,
                Address = model.Address,
                Country = model.Country,
                Description = model.Description,
                Price = model.Price,
                Status = model.Status.ToString(),
                Category = model.Category.ToString(),
                Image = model.Image,
            };

            return this.View(viewModel);
        }

        [ActionName("AllHouses")]
        public async Task<IActionResult> AllHousesAsync()
        {
            var category = HomeCategory.House;
            var viewModel = new PropertyListByCategoryViewModel
            {
                Properties = await this.listingService.GetAllByCategoryAsync(category),
                Category = category.ToString(),
            };

            return this.View(viewModel);
        }

        [ActionName("AllApartments")]
        public async Task<IActionResult> AllApartmentsAsync()
        {
            var category = HomeCategory.Apartment;

            var viewModel = new PropertyListByCategoryViewModel
            {
                Properties = await this.listingService.GetAllByCategoryAsync(category),
                Category = category.ToString(),
            };

            return this.View(viewModel);
        }

        [ActionName("AllRooms")]
        public async Task<IActionResult> AllRoomsAsync()
        {
            var category = HomeCategory.Room;

            var viewModel = new PropertyListByCategoryViewModel
            {
                Properties = await this.listingService.GetAllByCategoryAsync(category),
                Category = category.ToString(),
            };

            return this.View(viewModel);
        }

        [ActionName("AllToRent")]
        public async Task<IActionResult> AllToRentAsync()
        {
            var status = HomeStatus.ToRent.ToString();

            var viewModel = new PropertyListByStatusViewModel
            {
                Properties = await this.listingService.GetAllByStatusAsync(status),
                Status = status.ToString(),
            };

            return this.View(viewModel);
        }

        [ActionName("AllToManage")]
        public async Task<IActionResult> AllToManageAsync()
        {
            var status = HomeStatus.ToManage.ToString();

            var viewModel = new PropertyListByStatusViewModel
            {
                Properties = await this.listingService.GetAllByStatusAsync(status),
                Status = status.ToString(),
            };

            return this.View(viewModel);
        }
    }
}
