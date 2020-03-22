namespace RPM.Web.Areas.Management.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using RPM.Services.Admin;
    using RPM.Services.Common;
    using RPM.Web.Areas.Management.Models.Listings;

    public class ListingsController : ManagementController
    {
        private readonly IListingService listingService;
        private readonly ICityService cityService;
        private readonly ICountryService countryService;

        public ListingsController(
            IListingService listingService,
            ICityService cityService,
            ICountryService countryService)
        {
            this.listingService = listingService;
            this.cityService = cityService;
            this.countryService = countryService;
        }

        public async Task<IActionResult> Index()
        {
            return this.View();
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
            return this.View();
        }

        [ActionName("All")]
        public async Task<IActionResult> AllAsync(string id)
        {
            return this.View();
        }
    }
}
