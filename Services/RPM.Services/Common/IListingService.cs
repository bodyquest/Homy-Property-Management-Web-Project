namespace RPM.Services.Common
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using RPM.Services.Common.Models.Home;

    public interface IListingService
    {
        Task<IEnumerable<PropertyListServiceModel>> GetPropertiesAsync();

        Task<IEnumerable<PropertyListServiceModel>> GetAllByStatusAsync(string status);

        Task<PropertyCountServiceModel> GetPropertyCountByCategoryAsync(string category);

        Task<IEnumerable<PropertyListServiceModel>> FindAsync(string searchText);
    }
}
