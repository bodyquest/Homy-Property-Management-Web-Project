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
    using RPM.Data.Common.Repositories;
    using RPM.Data.Models;
    using RPM.Services.Admin.Models;
    using RPM.Services.Mapping;

    public class AdminUserService : IAdminUserService
    {
        private readonly ApplicationDbContext context;
        private readonly IDeletableEntityRepository<User> usersRepository;

        public AdminUserService(ApplicationDbContext context, Data.Common.Repositories.IDeletableEntityRepository<User> usersRepository)
        {
            this.context = context;
            this.usersRepository = usersRepository;
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

        public IEnumerable<T> GetAll<T>(int? count = null)
        {
            IQueryable<User> query = this.usersRepository.All();

            if (count.HasValue)
            {
                query = query.Take(count.Value);
            }

            return query.To<T>().ToList();
        }
    }
}
