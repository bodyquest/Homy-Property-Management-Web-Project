namespace RPM.Services.Admin.Implementations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using RPM.Data;
    using RPM.Data.Models;
    using RPM.Services.Admin.Models;
    using RPM.Web.Areas.Administration.Models.Cities;

    public class AdminCityService : IAdminCityService
    {
        private readonly ApplicationDbContext context;

        public AdminCityService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<AdminCitiesListingServiceModel>> AllCitiesAsync()
        {
            var cities = await this.context.Cities.Select(c => new AdminCitiesListingServiceModel
            {
                Id = c.Id,
                Name = c.Name,
                Country = c.Country.Name,
            })
            .OrderBy(c => c.Name)
            .ToListAsync();

            return cities;
        }

        public async Task<AdminCityAndCountryInputModel> GetCreateAsync()
        {
            AdminCityAndCountryInputModel model = new AdminCityAndCountryInputModel
            {
                CountryList = await this.context.
                    Countries
                    .OrderBy(c => c.Name)
                    .ToListAsync(),

                City = new City(),

                CityList = await this.context.Cities
                    .OrderBy(x => x.Name)
                    .Select(x => x.Name)
                    .Distinct()
                    .ToListAsync(),
            };

            return model;
        }

        public async Task<int?> CreateAsync(string name, int countryId)
        {
            var cityFromDb = await this.context.Cities
                .FirstOrDefaultAsync(s => s.Name == name && s.CountryId == countryId);

            if (cityFromDb != null)
            {
                return null;
            }
            else
            {
                var city = new City
                {
                    Name = name,
                    CountryId = countryId,
                };

                await this.context.Cities.AddAsync(city);
                await this.context.SaveChangesAsync();

                return city.Id;
            }
        }

        public async Task<AdminCityEditDeleteServiceModel> GetUpdateAsync(int? id)
        {
            var city = await this.context.Cities
                .FindAsync(id);

            if (city == null)
            {
                return null;
            }

            var country = await this.context.Countries.FindAsync(city.CountryId);

            var model = new AdminCityEditDeleteServiceModel
            {
                Country = country.Name,

                City = city,

                CityList = await this.context.Cities
                    .Where(x => x.CountryId == city.CountryId)
                    .OrderBy(x => x.Name)
                    .Select(x => x.Name)
                    .Distinct()
                    .ToListAsync(),
            };

            return model;
        }

        public async Task<bool> UpdateAsync(int? id, string name)
        {
            var city = await this.context.Cities
                .FindAsync(id);

            if (city == null || string.IsNullOrWhiteSpace(name))
            {
                return false;
            }

            city.Name = name;

            await this.context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int? id)
        {
            throw new NotImplementedException();
        }

        public async Task<AdminCityEditDeleteServiceModel> GetByIdAsync(int? id)
        {
            throw new NotImplementedException();
        }
    }
}
