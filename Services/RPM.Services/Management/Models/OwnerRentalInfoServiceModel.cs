namespace RPM.Services.Management.Models
{
    using System;

    public class OwnerRentalInfoServiceModel
    {
        public string RentalDate { get; set; }

        public int? Duration { get; set; }

        public string TenantFullName { get; set; }

        public string ManagerFullName { get; set; }
    }
}
