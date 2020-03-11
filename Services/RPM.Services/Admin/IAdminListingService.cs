namespace RPM.Services.Admin
{
    using RPM.Services.Admin.Models;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    public interface IAdminListingService
    {
        Task<IEnumerable<AdminHomesListingServiceModel>> GetAllListingsAsync();

        Task<bool> CreateListingAsync(AdminHomeCreateServiceModel model);
    }
}
