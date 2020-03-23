namespace RPM.Services.Management.Implementations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
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
        private readonly UserManager<User> userManager;
        private readonly RoleManager<ApplicationRole> roleManager;

        public OwnerRentalService(
            ApplicationDbContext context,
            UserManager<User> userManager
            )
        {
            this.context = context;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        public async Task<bool> StartRent(string id, byte[] fileContent)
        {
            var request = await this.context.Requests
                .Where(r => r.Id == id)
                .FirstOrDefaultAsync();

            // Approve Request
            request.IsApproved = true;

            var homeId = request.HomeId;
            var home = await this.context.Homes.FindAsync(homeId);
            home.Status = (HomeStatus)Enum.Parse(typeof(HomeStatus), Managed);

            var userId = request.UserId;
            var user = await this.userManager.FindByIdAsync(userId);

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
            var contractEntity = new Contract
            {
                Title = $"{DateTime.UtcNow.Year}_{user.UserName}",
                ContractDocument = fileContent,
                RentalId = rental.Id,
            };
            this.context.Contracts.Add(contractEntity);

            // Add Rental to Tenant
            user.Rentals.Add(rental);

            result = await this.context.SaveChangesAsync();
            if (result == 0)
            {
                return false;
            }

            return true;
        }

        public async Task<IEnumerable<OwnerIndexRentalServiceModel>> GetRentalsAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<OwnerRentalInfoServiceModel> GetRentalAsync()
        {
            throw new NotImplementedException();
        }
    }
}
