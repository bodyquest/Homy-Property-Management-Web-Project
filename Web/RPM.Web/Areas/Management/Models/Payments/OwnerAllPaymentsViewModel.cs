namespace RPM.Web.Areas.Management.Models.Payments
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using RPM.Services.Management.Models;

    public class OwnerAllPaymentsViewModel
    {
        public IEnumerable<OwnerAllPaymentsServiceModel> Payments { get; set; }
    }
}
