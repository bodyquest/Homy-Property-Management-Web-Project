namespace RPM.Services.Admin.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using RPM.Data.Models;

    public class AdminCityAndCountryViewModel
    {
        public City City { get; set; }

        public IEnumerable<Country> CountryList { get; set; }

        public List<string> CityList { get; set; }

        public string StatusMessage { get; set; }
    }
}
