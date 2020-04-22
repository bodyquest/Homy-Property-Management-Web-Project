namespace RPM.Services.Management
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using RPM.Data.Models;
    using RPM.Services.Management.Models;

    public interface IOwnerListingService
    {
        Task<IEnumerable<OwnerIndexListingsServiceModel>> GetMyPropertiesAsync(string id);

        Task<bool> CreateListingAsync(OwnerCreateListingServiceModel model);

        Task<bool> DeleteAsync(string id);

        Task<bool> EditListingAsync(OwnerEditListingServiceModel model);

        Task<OwnerListingFullDetailsServiceModel> GetDetailsAsync(string userId, string id);

        Task<OwnerListingFullDetailsServiceModel> GetEditModelAsync(string userId, string id);

        Task<IEnumerable<OwnerPropertyWithDetailsServiceModel>> GetMyPropertiesWithDetailsAsync(string userId);

        Task<string> ChangeHomeStatusAsync(Request request);

        Task<bool> StartHomeManage(string id, byte[] fileContent);

        Task<bool> StopHomeManageAsync(string id);

        Task<IEnumerable<OwnerTransactionListOfManagedHomesServiceModel>>
           GetManagedHomesAsync(string userId);

        Task<bool> IsHomeDeletable(string id);
    }
}
