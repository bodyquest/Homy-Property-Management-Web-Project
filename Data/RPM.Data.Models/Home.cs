namespace RPM.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Text;

    using RPM.Data.Models.Enums;

    using static RPM.Common.GlobalConstants;

    public class Home
    {
        public Home()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Images = new HashSet<CloudImage>();
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
        [RegularExpression(RegexAddress, ErrorMessage = RegexAddressError)]
        public string Address { get; set; }

        [Range(typeof(decimal), PriceMin, PriceMax)]
        public decimal Price { get; set; }

        [Required]
        public HomeStatus Status { get; set; }

        [Required]
        public HomeCategory Category { get; set; }

        [Required]
        [ForeignKey(nameof(User))]
        public string OwnerId { get; set; }
        public User Owner { get; set; }

        //[ForeignKey(nameof(User))]
        //public string ManagerId { get; set; }
        //public User Manager { get; set; }

        [ForeignKey(nameof(City))]
        public int CityId { get; set; }
        public City City { get; set; }

        public ICollection<CloudImage> Images { get; set; }
    }
}
