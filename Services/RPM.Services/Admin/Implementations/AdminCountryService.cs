namespace RPM.Services.Admin.Implementations
{
    using Microsoft.EntityFrameworkCore;
    using RPM.Data;
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

        public Task<bool> CreateAsync(string name)
        {
            throw new NotImplementedException();
        }
    }
}
