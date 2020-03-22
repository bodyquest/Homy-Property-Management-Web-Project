namespace RPM.Services.Common.Implementations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using RPM.Data;
    using RPM.Services.Common.Models.Country;

    public class CountryService : ICountryService
    {
        private readonly ApplicationDbContext context;

        public CountryService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<CountryListServiceModel>> AllCountriesAsync ()
        {
            var countries = await this.context.Countries
                .Select(c => new CountryListServiceModel
                {
                    Id = c.Id,
                    Name = c.Name,
                })
                .ToListAsync();

            return countries;
        }
    }
}
