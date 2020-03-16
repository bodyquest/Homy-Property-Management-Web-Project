namespace RPM.Web.Models.Home
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using RPM.Services.Common.Models.Home;

    public class HomeIndexViewModel : SearchFormModel
    {

        public IEnumerable<PropertyListServiceModel> Properties { get; set; }
    }
}
