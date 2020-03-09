namespace RPM.Services.Admin.Models
{
    using System.Collections.Generic;
    using RPM.Data.Models;

    public class AdminCityEditDeleteServiceModel
    {
        public City City { get; set; }

        public string Country { get; set; }

        public List<string> CityList { get; set; }

        public string StatusMessage { get; set; }
    }
}
