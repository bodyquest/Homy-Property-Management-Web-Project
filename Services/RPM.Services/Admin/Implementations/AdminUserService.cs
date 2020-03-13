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

    using static RPM.Common.GlobalConstants;

    public class AdminUserService : IAdminUserService
    {
        private readonly ApplicationDbContext context;
        private readonly IDeletableEntityRepository<User> usersRepository;

        public AdminUserService(ApplicationDbContext context, Data.Common.Repositories.IDeletableEntityRepository<User> usersRepository)
        {
            this.context = context;
            this.usersRepository = usersRepository;
        }

        public async Task<IEnumerable<AdminUserListingServiceModel>> AllAsync(int page = 1)
        {
            return await this.context.Users
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .Select(x => new AdminUserListingServiceModel
                {
                    Username = x.UserName,
                    Email = x.Email,
                    RegisteredOn = x.CreatedOn,

                })
                .ToListAsync();
        }

        public int Total() => this.context.Users.Count();

        public IEnumerable<T> GetAll<T>(int? count = null)
        {
            IQueryable<User> query = this.usersRepository.All();

            if (count.HasValue)
            {
                query = query.Take(count.Value);
            }

            return query.To<T>().ToList();
        }

        public async Task<User> GetUserByUsername(string username)
        {
            var user = await this.context.Users.FirstOrDefaultAsync(u => u.UserName == username);

            if (user == null)
            {
                return null;
            }

            return user;
        }
    }
}
