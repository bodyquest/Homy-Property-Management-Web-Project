namespace RPM.Web.Models.Dashboard
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using RPM.Services.Common.Models.Dashboard;
    using RPM.Services.Common.Models.Listing;

    public class ManagerDashboardViewModel
    {
        public bool HasStripeAccount { get; set; }

        public IEnumerable<ManagerDashboardPropertiesServiceModel> ManagedProperties { get; set; }

        public IEnumerable<ManagerPaymentListServiceModel> Payments { get; set; }
    }
}
