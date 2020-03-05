namespace RPM.Data.Models
{
    using RPM.Data.Models.Enums;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Text;

    using static RPM.Common.GlobalConstants;

    public class Home
    {
        public Home()
        {
            this.Id = Guid.NewGuid().ToString();
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

        [Required]
        [MinLength(HomeAddressMin)]
        [MaxLength(HomeAddressMax)]
        public string Address { get; set; }

        [Range(typeof(decimal), PriceMin, PriceMax)]
        public decimal Price { get; set; }

        [Required]
        public HomeStatus Status { get; set; }

        [Required]
        public HomeCategory Category { get; set; }

        [ForeignKey(nameof(User))]
        public string ManagerId { get; set; }
        public User Manager { get; set; }

        [ForeignKey(nameof(Country))]
        public int CountryId { get; set; }
        public Country Country { get; set; }
    }
}
