namespace RPM.Services.Admin.Implementations
{
    using Microsoft.EntityFrameworkCore;
    using RPM.Data;
    using RPM.Data.Models;
    using RPM.Services.Admin.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

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

        public Task<bool> UpdateAsync(int? id, string name, string code)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int? id)
        {
            throw new NotImplementedException();
        }
    }
}
