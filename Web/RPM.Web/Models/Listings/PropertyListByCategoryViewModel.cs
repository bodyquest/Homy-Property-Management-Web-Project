namespace RPM.Web.Models.Listings
{
    using System.Collections.Generic;
    using RPM.Services.Common.Models.Home;

    public class PropertyListByCategoryViewModel
    {
        public IEnumerable<PropertyListServiceModel> Properties { get; set; }

        public string Category { get; set; }
    }
}
