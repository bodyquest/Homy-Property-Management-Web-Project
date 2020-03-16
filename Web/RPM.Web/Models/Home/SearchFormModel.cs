namespace RPM.Web.Models.Home
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using RPM.Services.Common.Models.Home;

    public class SearchFormModel
    {
        public string SearchText { get; set; }

        public bool SearchInCities { get; set; }
    }
}
