namespace RPM.Services.Common
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using RPM.Services.Common.Models.Country;

    public interface ICountryService
    {
        Task<IEnumerable<CountryListServiceModel>> AllCountriesAsync();
    }
}
