namespace RPM.Web.Areas.Management.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using RPM.Data.Models;
    using RPM.Services.Admin;
    using RPM.Services.Common;
    using RPM.Services.Management;
    using RPM.Web.Areas.Management.Models.Listings;

    public class ListingsController : ManagementController
    {
        private readonly IOwnerListingService listingService;
        private readonly ICityService cityService;
        private readonly ICountryService countryService;
        private readonly UserManager<User> userManager;

        public ListingsController(
            IOwnerListingService listingService,
            ICityService cityService,
            ICountryService countryService,
            UserManager<User> userManager)
        {
            this.listingService = listingService;
            this.cityService = cityService;
            this.countryService = countryService;
            this.userManager = userManager;
        }

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

        [ActionName("Create")]
        public async Task<IActionResult> Create()
        {
            var cities = await this.cityService.AllCitiesAsync();
            var countries = await this.countryService.AllCountriesAsync();

            var viewModel = new OwnerListingCreateInputModel
            {
                Cities = cities,
                Countries = countries,
            };

            return this.View(viewModel);
        }

        [ActionName("GetCity")]
        public async Task<IActionResult> GetCityAsync(int id)
        {
            var model = await this.cityService.AllCitiesByCountryAsync(id);

            return this.Json(new SelectList(model, "Id", "Name"));
        }

        [HttpPost]
        [ActionName("Create")]
        public async Task<IActionResult> CreatePostAsync()
        {
            var viewModel = new OwnerListingCreateInputModel();

            return this.View(viewModel);
        }

        [ActionName("Details")]
        public async Task<IActionResult> DetailsAsync(string id)
        {
            var userId = this.userManager.GetUserId(this.User);
            var model = await this.listingService.GetDetailsAsync(userId, id);

            return this.View(model);
        }

        [ActionName("All")]
        public async Task<IActionResult> AllAsync(string id)
        {
            return this.View();
        }
    }
}
