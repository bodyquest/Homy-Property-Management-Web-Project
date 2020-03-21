namespace RPM.Services.Management.Models
{
    using RPM.Data.Models.Enums;
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class OwnerIndexListingsServiceModel
    {
        public string Id { get; set; }

        public string City { get; set; }

        public string Address { get; set; }

        public string Category { get; set; }

        public string Status { get; set; }
    }
}
