namespace RPM.Web.Areas.Administration.Models.Listings
{
    using AutoMapper;
    using Microsoft.AspNetCore.Http;
    using RPM.Data.Models.Enums;
    using RPM.Web.Areas.Administration.Models.Countries;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;

    using static RPM.Common.GlobalConstants;

    public class AdminListingCreateInputModel
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

        [Required]
        public HomeStatus Status { get; set; }

        [Required]
        public HomeCategory Category { get; set; }

        [Required]
        public string Owner { get; set; }

        [Required]
        public string Country { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public IFormFile Image { get; set; }

        public IEnumerable<CityViewModel> Cities { get; set; }

        public IEnumerable<CountryViewModel> Countries { get; set; }

        public IEnumerable<IFormFile> Images { get; set; }
    }
}
