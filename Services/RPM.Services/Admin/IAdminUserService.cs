namespace RPM.Services.Admin
{
    using RPM.Services.Admin.Models;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    public interface IAdminUserService
    {
        Task<IEnumerable<AdminUserListingServiceModel>> AllAsync();

        IEnumerable<T> GetAll<T>(int? count = null);
    }
}
