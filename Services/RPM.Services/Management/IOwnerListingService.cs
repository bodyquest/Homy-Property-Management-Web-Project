namespace RPM.Services.Management
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using RPM.Services.Management.Models;

    public interface IOwnerListingService
    {
        Task<IEnumerable<OwnerIndexListingsServiceModel>> GetMyPropertiesAsync(string id);

        Task<OwnerListingFullDetailsServiceModel> GetDetailsAsync(string userId, string id);

        Task<IEnumerable<OwnerPropertyWithDetailsServiceModel>> GetMyPropertiesWithDetailsAsync(string userId);
    }
}
