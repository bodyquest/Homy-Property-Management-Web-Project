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
    using RPM.Web.Areas.Administration.Models.Countries;

    public class AdminCountryService : IAdminCountryService
    {
        private readonly ApplicationDbContext context;

        public AdminCountryService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<AdminCountriesListingServiceModel>> AllCountriesAsync()
        {
            var countries = await this.context.Countries.Select(c => new AdminCountriesListingServiceModel
            {
                Id = c.Id,
                Name = c.Name,
                Code = c.Code,
            })
            .OrderBy(c => c.Name)
            .ToListAsync();

            return countries;
        }

        public async Task<bool> CreateAsync(string name, string code)
        {
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(code))
            {
                return false;
            }

            var country = new Country
            {
                Name = name,
                Code = code.ToUpper(),
            };

            await this.context.Countries.AddAsync(country);
            var result = await this.context.SaveChangesAsync();

            if (result != 0)
            {
                return true;
            }

            return false;
        }

        public async Task<bool> UpdateAsync(int? id, string name, string code)
        {
            var country = await this.context.Countries.FindAsync(id);

            if (id == null || string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(code))
            {
                return false;
            }

            country.Name = name;
            country.Code = code;

            this.context.Countries.Update(country);
            var result = await this.context.SaveChangesAsync();

            if (result != 0)
            {
                return true;
            }

            return false;
        }

        public async Task<bool> DeleteAsync(int? id)
        {
            var country = await this.context.Countries.FindAsync(id);

            if (country == null)
            {
                return false;
            }

            this.context.Countries.Remove(country);
            var result = await this.context.SaveChangesAsync();

            if (result != 0)
            {
                return true;
            }

            return false;
        }

        public async Task<AdminCountryEditDeleteServiceModel> GetByIdAsync(int? id)
        {
            var country = await this.context.Countries.FindAsync(id);

            if (country == null)
            {
                return null;
            }

            var model = new AdminCountryEditDeleteServiceModel
            {
                Id = country.Id,
                Name = country.Name,
                Code = country.Code,
            };

            return model;
        }
    }
}
