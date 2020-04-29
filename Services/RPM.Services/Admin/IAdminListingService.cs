namespace RPM.Services.Admin
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    using RPM.Services.Admin.Models;

    public interface IAdminListingService
    {
        Task<IEnumerable<AdminHomesListingServiceModel>> GetAllListingsAsync();

        Task<bool> CreateListingAsync(AdminHomeCreateServiceModel model);
    }
}
