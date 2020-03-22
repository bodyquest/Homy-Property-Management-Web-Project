namespace RPM.Services.Common.Models.City
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class CityListServiceModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int CountryId { get; set; }
    }
}
