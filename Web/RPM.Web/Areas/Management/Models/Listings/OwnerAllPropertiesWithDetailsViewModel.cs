namespace RPM.Web.Areas.Management.Models.Listings
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using RPM.Services.Management.Models;

    public class OwnerAllPropertiesWithDetailsViewModel
    {
        public IEnumerable<OwnerPropertyWithDetailsServiceModel> MyProperties { get; set; }
    }
}
