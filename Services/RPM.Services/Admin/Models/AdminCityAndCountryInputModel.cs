namespace RPM.Web.Areas.Administration.Models.Cities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using RPM.Data.Models;

    public class AdminCityAndCountryInputModel
    {
        public City City { get; set; }

        public IEnumerable<Country> CountryList { get; set; }

        public List<string> CityList { get; set; }

        public string StatusMessage { get; set; }
    }
}
