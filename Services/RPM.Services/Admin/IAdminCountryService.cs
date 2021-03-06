﻿namespace RPM.Services.Admin
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    using RPM.Data.Models;
    using RPM.Services.Admin.Models;
    using RPM.Web.Areas.Administration.Models.Countries;

    public interface IAdminCountryService
    {
        Task<IEnumerable<AdminCountriesListingServiceModel>> AllCountriesAsync();

        Task<bool> CreateAsync(string name, string code);

        Task<bool> UpdateAsync(int? id, string name, string code);

        Task<bool> DeleteAsync(int? id);

        Task<AdminCountryEditDeleteServiceModel> GetByIdAsync(int? id);
    }
}
