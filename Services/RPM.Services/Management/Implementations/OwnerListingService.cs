namespace RPM.Services.Management.Implementations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
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
        private readonly IOwnerRequestService requestService;
        private readonly IOwnerContractService contractService;

        public OwnerListingService(
            ApplicationDbContext context,
            UserManager<User> userManager,
            IOwnerRequestService requestService,
            IOwnerContractService contractService)
        {
            this.context = context;
            this.userManager = userManager;
            this.requestService = requestService;
            this.contractService = contractService;
        }

        public async Task<IEnumerable<OwnerIndexListingsServiceModel>> GetMyPropertiesAsync(string id)
        {
            var homes = await this.context.Homes
                .Include(h => h.Manager)
                .Where(homes => homes.OwnerId == id)
                .Select(h => new OwnerIndexListingsServiceModel
                {
                    Id = h.Id,
                    City = h.City.Name,
                    Address = h.Address,
                    Category = h.Category.ToString(),
                    Status = h.Status.ToString(),
                    Manager = string.Format(
                        ManagerFullName, h.Manager.FirstName, h.Manager.LastName),
                    Tenant = this.context.Rentals
                    .Where(r => r.HomeId == h.Id)
                    .Select(r => string.Format(
                        TenantFullName, r.Tenant.FirstName, r.Tenant.LastName))
                    .FirstOrDefault(),
                })
                .ToListAsync();

            return homes;
        }

        public async Task<bool> CreateListingAsync(OwnerCreateListingServiceModel model)
        {
            var city = await this.context.Cities.FirstOrDefaultAsync(x => x.Id == model.CityId);

            Home home = new Home
            {
                Name = model.Name,
                Description = model.Description,
                Address = model.Address,
                Price = model.Price,
                City = city,
                Category = model.Category,
                Status = model.Status,
                Owner = model.Owner,
            };

            home.Images.Add(model.Image);

            this.context.Homes.Add(home);
            var result = await this.context.SaveChangesAsync();

            city.Homes.Add(home);
            await this.context.SaveChangesAsync();

            return result > 0;
        }

        public async Task<bool> EditListingAsync(OwnerEditListingServiceModel model)
        {
            var home = await this.context.Homes.FirstOrDefaultAsync(h => h.Id == model.Id);

            home.Name = model.Name;
            home.Description = model.Description;
            home.Price = model.Price;
            home.Category = model.Category;
            home.Status = model.Status;

            if (model.Image != null)
            {
                home.Images.Add(model.Image);
            }

            this.context.Update(home);
            var result = await this.context.SaveChangesAsync();

            return result > 0;
        }

        public async Task<OwnerListingFullDetailsServiceModel> GetDetailsAsync(string userId, string id)
        {
            var rental = await this.context.Rentals
                .Where(r => r.HomeId == id)
                .Include(r => r.Contract)
                .Include(r => r.Tenant)
                .Select(r => new OwnerRentalInfoServiceModel
                {
                    RentalDate = r.RentDate.ToString(StandartDateFormat),
                    Duration = r.Duration,
                    TenantFullName = string.Format(
                        TenantFullName, r.Tenant.FirstName, r.Tenant.LastName),
                    ManagerFullName = string.Format(
                        ManagerFullName, r.Home.Manager.FirstName, r.Home.Manager.LastName),
                })
                .FirstOrDefaultAsync();

            var model = await this.context.Homes
               .Where(h => h.OwnerId == userId && h.Id == id)
               .Include(x => x.City)
               .Include(x => x.Manager)
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
                   ManagerFullName = string.Format(
                       ManagerFullName, x.Manager.FirstName, x.Manager.LastName),
                   Price = x.Price,
                   Status = x.Status,
                   Category = x.Category,
                   Image = x.Images.Select(i => i.PictureUrl).FirstOrDefault(),
                   RentalInfo = rental,
               })
               .FirstOrDefaultAsync();

            return model;
        }

        public async Task<OwnerListingFullDetailsServiceModel> GetEditModelAsync(string userId, string id)
        {
            var rental = await this.context.Rentals
                .Where(r => r.HomeId == id)
                .Include(r => r.Home.Manager)
                .Include(r => r.Contract)
                .Include(r => r.Tenant)
                .Select(r => new OwnerRentalInfoServiceModel
                {
                    RentalDate = r.RentDate.ToString(StandartDateFormat),
                    Duration = r.Duration,
                    TenantFullName = string.Format(TenantFullName, r.Tenant.FirstName, r.Tenant.LastName),
                    ManagerFullName = string.Format(ManagerFullName, r.Home.Manager.FirstName, r.Home.Manager.LastName),
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

            if (request.Type == RequestType.ToRent)
            {
                home.Status = (HomeStatus)Enum.Parse(typeof(HomeStatus), Rented);
            }
            else if (request.Type == RequestType.ToManage)
            {
                home.Status = (HomeStatus)Enum.Parse(typeof(HomeStatus), Managed);
            }
            else if (request.Type == RequestType.CancelRent)
            {
                home.Status = (HomeStatus)Enum.Parse(typeof(HomeStatus), ToRent);
            }
            else
            {
                home.Status = (HomeStatus)Enum.Parse(typeof(HomeStatus), ToManage);
            }

            await this.context.SaveChangesAsync();

            return homeId;
        }

        public async Task<bool> StartHomeManage(string id, byte[] fileContent)
        {
            // Approve Request for Manage
            var request = await this.requestService.ApproveRequestAsync(id);

            if (request == null)
            {
                return false;
            }

            // Change Home Status
            string homeId = await this.ChangeHomeStatusAsync(request);

            if (homeId == EntityNotFound)
            {
                return false;
            }

            // Add User to Role
            var userId = request.UserId;
            var user = await this.userManager.FindByIdAsync(userId);
            await this.userManager.AddToRoleAsync(user, ManagerRoleName);

            // Add Contract
            var isSuccessful = await this.contractService
                .CreateManageContractAsync(fileContent, user);

            if (!isSuccessful)
            {
                return false;
            }

            // Add Manager to Home
            var home = await this.context.Homes
                .Where(h => h.Id == request.HomeId)
                .FirstOrDefaultAsync();

            home.ManagerId = user.Id;

            // Add Home to ManagedHomesCollection of Manager
            user.ManagedHomes.Add(home);

            var result = await this.context.SaveChangesAsync();

            return result > 0;
        }

        public async Task<IEnumerable<OwnerTransactionListOfManagedHomesServiceModel>>
           GetManagedHomesAsync(string userId)
        {
            var homes = await this.context.Homes
                .Include(h => h.City)
                .Where(h => h.OwnerId == userId && h.Manager != null)
                .Select(h => new OwnerTransactionListOfManagedHomesServiceModel
                {
                    Id = h.Id,
                    Name = string.Format(DashboardRequestLocation, h.City.Name, h.Address),
                })
                .ToListAsync();

            return homes;
        }

        public async Task<bool> IsHomeDeletable(string id)
        {
            var home = await this.context.Homes
                .FindAsync(id);

            var rental = await this.context.Rentals
                .Where(r => r.HomeId == id)
                .FirstOrDefaultAsync();

            if (home.Manager != null || rental != null)
            {
                return false;
            }
            else
            {
                this.context.Homes.Remove(home);
                var result = await this.context.SaveChangesAsync();

                return result > 0;
            }
        }
    }
}
