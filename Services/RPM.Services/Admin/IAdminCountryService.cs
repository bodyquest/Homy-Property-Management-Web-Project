namespace RPM.Services.Admin
{
    using RPM.Data.Models;
    using RPM.Services.Admin.Models;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    public interface IAdminCountryService
    {
        Task<IEnumerable<AdminCountriesListingServiceModel>> AllCountriesAsync();

        Task<bool> CreateAsync(string name);
    }
}
