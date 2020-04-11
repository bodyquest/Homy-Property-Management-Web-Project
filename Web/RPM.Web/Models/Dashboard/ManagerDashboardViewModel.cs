namespace RPM.Web.Models.Dashboard
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using RPM.Services.Common.Models.Listing;

    public class ManagerDashboardViewModel
    {
        public IEnumerable<ManagerDashboardPropertiesServiceModel> ManagedProperties { get; set; }

        //public IEnumerable<ManagerDashboardPaymentsServiceModel> Payments { get; set; }
    }
}
