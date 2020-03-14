namespace RPM.Services.Admin.Models
{
    using AutoMapper;
    using Microsoft.AspNetCore.Http;
    using RPM.Data.Models;
    using RPM.Data.Models.Enums;
    using RPM.Services.Mapping;
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class AdminHomeCreateServiceModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public decimal Price { get; set; }

        public HomeStatus Status { get; set; }

        public HomeCategory Category { get; set; }

        public string Owner { get; set; }

        public CloudImage Image { get; set; }

        public ICollection<CloudImage> Images { get; set; }
    }
}
