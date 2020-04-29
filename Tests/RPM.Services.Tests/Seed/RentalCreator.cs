namespace RPM.Services.Tests.Seed
{
    using System;
    using RPM.Data.Models;

    public class RentalCreator : BaseServiceTest
    {
        public static Rental Create(int id, Country country, City city, User tenant, Home home, Contract contract)
        {
            var rnd = new Random();

            return new Rental
            {
                Id = id,
                RentDate = DateTime.UtcNow,
                Home = home,
                Tenant = tenant,
                TenantId = tenant.Id,
                Duration = rnd.Next(6, 72),
                Contract = contract,
            };
        }
    }
}
