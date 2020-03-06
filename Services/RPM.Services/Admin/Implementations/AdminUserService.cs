namespace RPM.Services.Admin.Implementations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Microsoft.EntityFrameworkCore;
    using RPM.Data;
    using RPM.Services.Admin.Models;
    using RPM.Services.Mapping;

    public class AdminUserService : IAdminUserService
    {
        private readonly ApplicationDbContext context;

        public AdminUserService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<AdminUserListingServiceModel>> AllAsync()
        {
            return await this.context.Users
                .Select(x => new AdminUserListingServiceModel
                {
                    Username = x.UserName,
                    Email = x.Email,

                })
                .ToListAsync();
        }


    }
}
