namespace RPM.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using RPM.Data.Models;

    public class CountriesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext context, IServiceProvider serviceProvider)
        {
            if (context.Countries.Any())
            {
                return;
            }

            var countries = new List<(string Name, string Code)>
            {
                ("Bulgaria", "BG"),
                ("Germany", "DE"),
                ("France", "FR"),
                ("Italy", "IT"),
                ("Portugal", "PT"),
            };

            foreach (var country in countries)
            {
                await context.Countries.AddAsync(new Country
                {
                    Name = country.Name,
                    Code = country.Code,
                });
            }
        }
    }
}
