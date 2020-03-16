namespace RPM.Web.Models.Home
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using RPM.Services.Common.Models.Home;

    public class HomeIndexViewModel : SearchFormModel
    {
        public HomeIndexViewModel()
        {
            this.PropertiesByCategory = new HashSet<PropertyCountServiceModel>();
        }

        public IEnumerable<PropertyListServiceModel> PropertiesToRent { get; set; }

        public IEnumerable<PropertyListServiceModel> PropertiesToManage { get; set; }

        public ICollection<PropertyCountServiceModel> PropertiesByCategory { get; set; }
    }
}
