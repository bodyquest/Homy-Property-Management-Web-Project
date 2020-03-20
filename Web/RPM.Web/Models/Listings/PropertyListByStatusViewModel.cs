namespace RPM.Web.Models.Listings
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using RPM.Services.Common.Models.Home;

    public class PropertyListByStatusViewModel
    {
        public IEnumerable<PropertyListServiceModel> Properties { get; set; }

        public string Status { get; set; }
    }
}
