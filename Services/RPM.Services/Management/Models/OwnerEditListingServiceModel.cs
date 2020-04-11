namespace RPM.Services.Management.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using RPM.Data.Models;
    using RPM.Data.Models.Enums;

    public class OwnerEditListingServiceModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public HomeStatus Status { get; set; }

        public HomeCategory Category { get; set; }

        public User Owner { get; set; }

        public CloudImage Image { get; set; }

        public ICollection<CloudImage> Images { get; set; }
    }
}
