namespace RPM.Services.Management.Implementations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using RPM.Data;
    using RPM.Data.Models;
    using RPM.Data.Models.Enums;
    using RPM.Services.Management.Models;

    using static RPM.Common.GlobalConstants;

    public class OwnerListingService : IOwnerListingService
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<User> userManager;

        public OwnerListingService(
            ApplicationDbContext context,
            UserManager<User> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        public async Task<IEnumerable<OwnerIndexListingsServiceModel>> GetMyPropertiesAsync(string id)
        {
            var homes = await this.context.Homes
                .Where(homes => homes.OwnerId == id)
                .Select(h => new OwnerIndexListingsServiceModel
                {
                    Id = h.Id,
                    City = h.City.Name,
                    Address = h.Address,
                    Category = h.Category.ToString(),
                    Status = h.Status.ToString(),
                })
                .ToListAsync();

            return homes;
        }

        public async Task<OwnerListingFullDetailsServiceModel> GetDetailsAsync(string userId, string id)
        {
            var rental = await this.context.Rentals
                .Where(r => r.HomeId == id)
                .Include(r => r.Contract)
                .Include(r => r.Tenant)
                .Include(r => r.Manager)
                .Select(r => new OwnerRentalInfoServiceModel
                {
                    RentalDate = r.RentDate.ToString(StandartDateFormat),
                    Duration = r.Duration,
                    TenantFullName = r.Tenant.FirstName + " " + r.Tenant.LastName,
                    ManagerFullName = r.Manager.FirstName + " " + r.Manager.LastName,
                })
                .FirstOrDefaultAsync();

            var model = await this.context.Homes
               .Where(h => h.OwnerId == userId && h.Id == id)
               .Include(x => x.City)
               .Include(x => x.City.Country)
               .Include(h => h.Images)
               .Select(x => new OwnerListingFullDetailsServiceModel
               {
                   Id = x.Id,
                   Name = x.Name,
                   City = x.City.Name,
                   Country = x.City.Country.Name,
                   Address = x.Address,
                   Description = x.Description,
                   Price = x.Price,
                   Status = x.Status,
                   Category = x.Category,
                   Image = x.Images.Select(i => i.PictureUrl).FirstOrDefault(),
                   RentalInfo = rental,
               })
               .FirstOrDefaultAsync();

            return model;
        }

        public async Task<IEnumerable<OwnerPropertyWithDetailsServiceModel>> GetMyPropertiesWithDetailsAsync(string userId)
        {
            var model = await this.context.Homes
               .Where(h => h.OwnerId == userId)
               .Include(x => x.City)
               .Include(x => x.City.Country)
               .Include(h => h.Images)
               .Select(x => new OwnerPropertyWithDetailsServiceModel
               {
                   Id = x.Id,
                   Name = x.Name,
                   City = x.City.Name,
                   Country = x.City.Country.Name,
                   Address = x.Address,
                   Description = x.Description,
                   Price = x.Price,
                   Status = x.Status,
                   Category = x.Category,
                   Image = x.Images.Select(i => i.PictureUrl).FirstOrDefault(),
               })
               .ToListAsync();

            return model;
        }

        public async Task<string> ChangeHomeStatusAsync(Request request)
        {
            var homeId = request.HomeId;
            var home = await this.context.Homes.FindAsync(homeId);

            if (home == null)
            {
                return EntityNotFound;
            }

            home.Status = (HomeStatus)Enum.Parse(typeof(HomeStatus), Managed);
            return homeId;
        }
    }
}
