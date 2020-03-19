namespace RPM.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
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

            if (viewModel == null)
            {
                return this.NotFound();
            }

            return this.View(viewModel);
        }
    }
}
