namespace RPM.Web.Areas.Management.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using RPM.Services.Management.Models;

    public class OwnerDashboardViewModel
    {
        public IEnumerable<OwnerIndexListingsServiceModel> Properties { get; set; }

        public IEnumerable<OwnerIndexRequestsServiceModel> Requests { get; set; }

        public IEnumerable<OwnerIndexRentalServiceModel> Rentals { get; set; }
    }
}
