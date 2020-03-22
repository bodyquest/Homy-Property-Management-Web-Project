namespace RPM.Services.Common
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using RPM.Services.Common.Models.City;

    public interface ICityService
    {
        Task<IEnumerable<CityListServiceModel>> AllCitiesAsync();

        Task<IEnumerable<CityListServiceModel>> AllCitiesByCountryAsync(int id);
    }
}
