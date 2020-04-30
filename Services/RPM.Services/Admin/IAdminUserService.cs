namespace RPM.Services.Admin
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    using RPM.Data.Models;
    using RPM.Services.Admin.Models;
    using RPM.Services.Common.Models.User;

    public interface IAdminUserService
    {
        Task<IEnumerable<AdminUserListingServiceModel>> AllAsync(int page = 1);

        Task<int> GetUsersCount();

        Task<UserProfileServiceModel> GetUserDetailsAsync(string id);

        int Total();

        IEnumerable<T> GetAll<T>(int? count = null);

        Task<User> GetUserByUsername(string username);
    }
}
