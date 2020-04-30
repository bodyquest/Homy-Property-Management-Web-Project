namespace RPM.Services.Admin
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using RPM.Services.Admin.Models;

    public interface IAdminListingService
    {
        Task<IEnumerable<AdminHomesListingServiceModel>> GetAllListingsAsync();

        Task<int> GetListingsCount();

        Task<bool> CreateListingAsync(AdminHomeCreateServiceModel model);
    }
}
