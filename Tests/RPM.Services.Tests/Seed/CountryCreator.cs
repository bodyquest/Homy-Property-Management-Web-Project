namespace RPM.Services.Tests.Seed
{
    using System;
    using RPM.Data.Models;

    public class CountryCreator
    {
        public static Country Create()
        {
            var rnd = new Random();

            return new Country
            {
                Id = rnd.Next(1, 1000),
                Name = Guid.NewGuid().ToString(),
                Code = Guid.NewGuid().ToString().Substring(0, 3),
            };
        }
    }
}
