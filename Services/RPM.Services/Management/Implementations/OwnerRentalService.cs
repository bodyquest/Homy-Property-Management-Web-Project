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

    public class OwnerRentalService : IOwnerRentalService
    {
        private readonly ApplicationDbContext context;
        private readonly IOwnerRequestService requestService;
        private readonly IOwnerListingService listingService;
        private readonly IOwnerContractService contractService;
        private readonly UserManager<User> userManager;
        private readonly RoleManager<ApplicationRole> roleManager;

        public OwnerRentalService(
            ApplicationDbContext context,
            IOwnerRequestService requestService,
            IOwnerListingService listingService,
            IOwnerContractService contractService,
            UserManager<User> userManager
            )
        {
            this.context = context;
            this.requestService = requestService;
            this.listingService = listingService;
            this.contractService = contractService;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        public async Task<bool> StartRent(string id, byte[] fileContent)
        {
            // Approve Request
            var request = await this.requestService.ApproveRentRequestAsync(id);

            if (request == null)
            {
                return false;
            }

            // Change Home Status
            string homeId = await this.listingService.ChangeHomeStatusAsync(request);

            if (homeId == EntityNotFound)
            {
                return false;
            }

            // Add User to Role
            var userId = request.UserId;
            var user = await this.userManager.FindByIdAsync(userId);
            await this.userManager.AddToRoleAsync(user, TenantRole);

            // Create Rental
            var rental = new Rental
            {
                RentDate = DateTime.UtcNow,
                HomeId = homeId,
                TenantId = userId,
            };

            this.context.Rentals.Add(rental);
            var result = await this.context.SaveChangesAsync();

            if (result == 0)
            {
                return false;
            }

            // Add Contract
            var isSuccessful = await this.contractService
                .CreateRentalContractAsync(fileContent, user, rental);

            if (!isSuccessful)
            {
                return false;
            }

            // Add Rental to Tenant
            user.Rentals.Add(rental);

            result = await this.context.SaveChangesAsync();
            if (result == 0)
            {
                return false;
            }

            return true;
        }

       



        public async Task<IEnumerable<OwnerIndexRentalServiceModel>> GetRentalsAsync(string userId)
        {
            var rentalsFromDb = await this.context.Rentals
                .Where(rentals => rentals.Home.OwnerId == userId)
                .OrderBy(r => r.RentDate)
                .ThenBy(r => r.Duration != null)
                .Select(r => new OwnerIndexRentalServiceModel
                {
                    Id = r.Id,
                    StartDate = r.RentDate.ToString("dd/MM/yyyy"),
                    Duration = r.Duration,
                    Address = string.Format(DashboardRentalLocation, r.Home.Address),
                    Tenant = string.Format(DashboardRentalFullName, r.Tenant.FirstName, r.Tenant.LastName),
                })
                .ToListAsync();

            return rentalsFromDb;
        }

        public async Task<OwnerRentalInfoServiceModel> GetRentalAsync()
        {
            throw new NotImplementedException();
        }
    }
}
