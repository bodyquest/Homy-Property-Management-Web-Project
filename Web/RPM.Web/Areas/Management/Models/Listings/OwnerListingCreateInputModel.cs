namespace RPM.Web.Areas.Management.Models.Listings
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using RPM.Data.Models;
    using RPM.Data.Models.Enums;
    using RPM.Services.Common.Models.City;
    using RPM.Services.Common.Models.Country;
    using static RPM.Common.GlobalConstants;

    public class OwnerListingCreateInputModel
    {
        public string Id { get; set; }

        [Required]
        [MinLength(HomeNameMin)]
        [MaxLength(HomeNameMax)]
        public string Name { get; set; }

        [Required]
        [MinLength(HomeDescriptionMin)]
        [MaxLength(HomeDescriptionMax)]
        public string Description { get; set; }

        [Required]
        [MinLength(HomeAddressMin)]
        [MaxLength(HomeAddressMax)]
        [RegularExpression(RegexAddress, ErrorMessage = RegexAddressError)]
        public string Address { get; set; }

        [Range(typeof(decimal), PriceMin, PriceMax)]
        public decimal Price { get; set; }

        [Display(Name = "City")]
        public int CityId { get; set; }

        public City City { get; set; }

        [Display(Name = "Country")]
        public int CountryId { get; set; }

        [Required]
        public HomeStatus Status { get; set; }

        [Required]
        public HomeCategory Category { get; set; }

        public bool UserHasStripeAccount { get; set; }

        [Required]
        public IFormFile Image { get; set; }

        public IEnumerable<CityListServiceModel> Cities { get; set; }

        public IEnumerable<CountryListServiceModel> Countries { get; set; }

        public IEnumerable<IFormFile> Images { get; set; }
    }
}
