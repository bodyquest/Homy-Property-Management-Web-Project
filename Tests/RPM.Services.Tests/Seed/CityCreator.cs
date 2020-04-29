namespace RPM.Services.Tests.Seed
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using RPM.Data.Models;

    public class CityCreator
    {
        public static City Create(int countryId)
        {
            var rnd = new Random();

            return new City
            {
                Id = rnd.Next(1, 1000),
                Name = Guid.NewGuid().ToString(),
                CountryId = countryId,
            };
        }
    }
}
