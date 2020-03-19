namespace RPM.Services.Common
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using RPM.Services.Common.Models.Home;
    using RPM.Services.Common.Models.Listing;

    public interface IListingService
    {
        Task<IEnumerable<PropertyListServiceModel>> GetPropertiesAsync();

        Task<PropertyDetailsServiceModel> GetDetailsAsync(string id);

        Task<IEnumerable<PropertyListServiceModel>> GetAllByStatusAsync(string status);

        Task<PropertyCountServiceModel> GetPropertyCountByCategoryAsync(string category);

        Task<IEnumerable<PropertyListServiceModel>> FindAsync(string searchText);
    }
}
