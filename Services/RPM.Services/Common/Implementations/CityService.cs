namespace RPM.Services.Common.Implementations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using RPM.Data;
    using RPM.Services.Common.Models.City;

    public class CityService : ICityService
    {
        private readonly ApplicationDbContext context;

        public CityService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<CityListServiceModel>> AllCitiesAsync()
        {
            var cities = await this.context.Cities
                .Select(c => new CityListServiceModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    CountryId = c.CountryId,
                })
                .ToListAsync();

            return cities;
        }

        public async Task<IEnumerable<CityListServiceModel>> AllCitiesByCountryAsync(int id)
        {
            var cities = await this.context.Cities
                .Where(c => c.CountryId == id)
                .Select(c => new CityListServiceModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    CountryId = c.CountryId,
                })
                .ToListAsync();

            return cities;
        }
    }
}
