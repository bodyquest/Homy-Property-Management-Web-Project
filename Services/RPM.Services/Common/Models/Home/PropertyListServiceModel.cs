namespace RPM.Services.Common.Models.Home
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using RPM.Data.Models.Enums;

    public class PropertyListServiceModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        public decimal Price { get; set; }

        public HomeStatus Status { get; set; }

        public HomeCategory Category { get; set; }

        public string Image { get; set; }
    }
}
