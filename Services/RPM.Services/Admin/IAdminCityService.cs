namespace RPM.Services.Admin
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    using RPM.Services.Admin.Models;
    using RPM.Web.Areas.Administration.Models.Cities;

    public interface IAdminCityService
    {
        Task<IEnumerable<AdminCitiesListingServiceModel>> AllCitiesAsync();


        Task<AdminCityAndCountryInputModel> GetCreateAsync();

        Task<int?> CreateAsync(string name, int countryId);


        Task<AdminCityEditDeleteServiceModel> GetUpdateAsync(int? id);

        Task<bool> UpdateAsync(int? id, string name);


        Task<bool> DeleteAsync(int? id);

        Task<AdminCityDetailsServiceModel> GetByIdAsync(int? id);
    }
}
