namespace RPM.Web.Areas.Management.Models.Listings
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Microsoft.AspNetCore.Http;
    using RPM.Data.Models.Enums;
    using RPM.Services.Management.Models;

    using static RPM.Common.GlobalConstants;

    public class OwnerListingEditInputModel
    {
        public OwnerListingEditInputModel()
        {
            this.Images = new HashSet<IFormFile>();
        }

        public string Id { get; set; }

        [Required]
        [MinLength(HomeNameMin)]
        [MaxLength(HomeNameMax)]
        public string Name { get; set; }

        [Required]
        [MinLength(HomeDescriptionMin)]
        [MaxLength(HomeDescriptionMax)]
        public string Description { get; set; }

        public string Address { get; set; }

        [Range(typeof(decimal), PriceMin, PriceMax)]
        public decimal Price { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        [Required]
        public HomeStatus Status { get; set; }

        [Required]
        public HomeCategory Category { get; set; }

        public string ImageFromDb { get; set; }

        public IFormFile Image { get; set; }

        public IEnumerable<IFormFile> Images { get; set; }

        public OwnerRentalInfoServiceModel RentalInfo { get; set; }
    }
}
