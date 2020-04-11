namespace RPM.Services.Common
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using RPM.Data.Models.Enums;
    using RPM.Services.Common.Models.Home;
    using RPM.Services.Common.Models.Listing;

    public interface IListingService
    {
        Task<IEnumerable<PropertyListServiceModel>> GetPropertiesAsync();

        Task<IEnumerable<ManagerDashboardPropertiesServiceModel>> GetManagedPropertiesAsync(string Id);

        Task<IEnumerable<PropertyListServiceModel>> GetAllByCategoryAsync(HomeCategory category);

        Task<PropertyDetailsServiceModel> GetDetailsAsync(string id);

        Task<IEnumerable<PropertyListServiceModel>> GetAllByStatusAsync(string status);

        Task<PropertyCountServiceModel> GetPropertyCountByCategoryAsync(string category);

        Task<IEnumerable<PropertyListServiceModel>> FindAsync(string searchText);

    }
}
