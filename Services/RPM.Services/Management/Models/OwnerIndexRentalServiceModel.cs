namespace RPM.Services.Management.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class OwnerIndexRentalServiceModel
    {
        public int Id { get; set; }

        public string StartDate { get; set; }

        public int? Duration { get; set; }

        public string Address { get; set; }

        public string Tenant { get; set; }
    }
}
