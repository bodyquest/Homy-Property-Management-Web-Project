namespace RPM.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using RPM.Data.Models;

    public class CitiesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext context, IServiceProvider serviceProvider)
        {
            if (context.Cities.Any())
            {
                return;
            }

            var cities = new List<(string Name, int CountryId)>
            {
                // Bulgaria
                ("Sofia", 1),
                ("Burgas", 1),
                ("Varna", 1),
                ("Plovdiv", 1),
                ("Pleven", 1),
                ("Veliko Turnovo", 1),
                ("Polski Trumbesh", 1),
                ("Kaspichan", 1),
                ("Petri4", 1),
                ("Lukovit", 1),

                // Germany
                ("Berlin", 2),
                ("Stutgart", 2),
                ("Dresden", 2),
                ("Frankfurt", 2),
                ("Leipzig", 2),

                // France
                ("Tours", 3),
                ("Paris", 3),
                ("Lion", 3),

                // Italy
                ("Florence", 4),
                ("Rome", 4),
                ("Milano", 4),

                // Portugal
                ("Porto", 5),
                ("Lisbon", 5),
                ("Ponta Delgada", 5),
            };

            foreach (var city in cities)
            {
                await context.Cities.AddAsync(new City
                {
                    Name = city.Name,
                    CountryId = city.CountryId,
                });
            }
        }
    }
}
