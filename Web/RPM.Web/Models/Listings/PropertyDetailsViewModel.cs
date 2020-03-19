namespace RPM.Web.Models.Listings
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using RPM.Data.Models.Enums;

    public class PropertyDetailsViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string City { get; set; }

        public string OwnerName { get; set; }

        public string Address { get; set; }

        public string Country { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public string Status { get; set; }

        public string Category { get; set; }

        public string Image { get; set; }
    }
}
