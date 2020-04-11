namespace RPM.Services.Common.Models.Rental
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class ManagerDashboardRentalServiceModel
    {
        public int Id { get; set; }

        public string HomeId { get; set; }

        public string TenantFullName { get; set; }
    }
}
