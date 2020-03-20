namespace RPM.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using RPM.Data.Models.Enums;
    using RPM.Services.Common;
    using RPM.Web.Models.Listings;

    public class ListingsController : BaseController
    {
        private readonly IListingService listingService;

        public ListingsController(IListingService listingService)
        {
            this.listingService = listingService;
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
