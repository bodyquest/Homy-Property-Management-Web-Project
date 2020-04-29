namespace RPM.Services.Tests.Seed
{
    using System;
    using RPM.Data.Models;
    using RPM.Data.Models.Enums;

    public class HomeCreator
    {
        public static Home Create(string ownerId, int cityId)
        {
            var rnd = new Random();

            return new Home
            {
                Name = Guid.NewGuid().ToString(),
                Description = Guid.NewGuid().ToString(),
                Address = $"{rnd.Next(5, 999)}, {Guid.NewGuid().ToString()}, {rnd.Next(1000, 9000)}",
                Price = rnd.Next(50, 10000),
                Status = (HomeStatus)rnd.Next(1, 4),
                Category = (HomeCategory)rnd.Next(1, 3),
                OwnerId = ownerId,
                CityId = cityId,
            };
        }

        public static Home CreateOwnerHome(string ownerId, int cityId)
        {
            var rnd = new Random();

            return new Home
            {
                Name = Guid.NewGuid().ToString(),
                Description = Guid.NewGuid().ToString(),
                Address = $"{rnd.Next(5, 999)}, {Guid.NewGuid().ToString()}, {rnd.Next(1000, 9000)}",
                Price = rnd.Next(50, 10000),
                Status = (HomeStatus)rnd.Next(1, 4),
                Category = (HomeCategory)rnd.Next(1, 3),
                OwnerId = ownerId,
                ManagerId = null,
                Manager = null,
                CityId = cityId,
            };
        }

        public static Home CreateManagedHome(string ownerId, int cityId)
        {
            var rnd = new Random();

            var home = new Home
            {
                Name = Guid.NewGuid().ToString(),
                Description = Guid.NewGuid().ToString(),
                Address = $"{rnd.Next(5, 999)}, {Guid.NewGuid().ToString()}, {rnd.Next(1000, 9000)}",
                Price = rnd.Next(50, 10000),
                Status = HomeStatus.Managed,
                Category = (HomeCategory)rnd.Next(1, 3),
                OwnerId = ownerId,
                Manager = UserCreator.Create("Kanalin", "Tsolov", "tsola", "kanalin@prasmail.com"),
                CityId = cityId,
            };

            home.ManagerId = home.Manager.Id;
            return home;
        }

        public static Home CreateAny(int cityId)
        {
            var rnd = new Random();

            return new Home
            {
                Name = Guid.NewGuid().ToString(),
                Description = Guid.NewGuid().ToString(),
                Address = $"{rnd.Next(5, 999)}, {Guid.NewGuid().ToString()}, {rnd.Next(1000, 9000)}",
                Price = rnd.Next(50, 999),
                Status = HomeStatus.ToRent,
                Category = HomeCategory.House,
                Owner = UserCreator.Create("Prasemir", "Butonoskov", "prasio", "prasi@prasmail.com"),
                CityId = cityId,
            };
        }
    }
}
