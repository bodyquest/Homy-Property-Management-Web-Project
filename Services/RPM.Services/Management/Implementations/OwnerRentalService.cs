namespace RPM.Services.Management.Implementations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Hangfire;
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

        public OwnerRentalService(
            ApplicationDbContext context,
            IOwnerRequestService requestService,
            IOwnerListingService listingService,
            IOwnerContractService contractService,
            UserManager<User> userManager)
        {
            this.context = context;
            this.requestService = requestService;
            this.listingService = listingService;
            this.contractService = contractService;
            this.userManager = userManager;
        }

        public async Task<bool> StartRent(string id, byte[] fileContent)
        {
            // Approve Request
            var request = await this.requestService.ApproveRequestAsync(id);

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

            await this.context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> StopRentAsync(string id)
        {
            // Approve Request
            var request = await this.requestService.ApproveRequestAsync(id);

            if (request == null)
            {
                return false;
            }

            var userId = request.UserId;
            var user = await this.userManager.FindByIdAsync(userId);

            // Change Home Status
            string homeId = await this.listingService.ChangeHomeStatusAsync(request);
            if (homeId == EntityNotFound)
            {
                return false;
            }

            var rental = await this.context.Rentals
                .Where(r => r.TenantId == user.Id && r.HomeId == homeId)
                .FirstOrDefaultAsync();

            // Remove user from Rental and remove Rental entity
            var userRentals = await this.context.Rentals.Where(r => r.TenantId == user.Id).ToListAsync();

            // Stop Recurring payments from Tenant to Owner
            List<string> transactionRequests = await this.context.TransactionRequests
                .Where(tr => tr.RentalId == rental.Id)
                .Select(tr => tr.Id)
                .ToListAsync();

            foreach (var transactionId in transactionRequests)
            {
                var tr = await this.context.TransactionRequests
                    .Where(tr => tr.Id == transactionId)
                    .FirstOrDefaultAsync();
                var payments = await this.context.Payments
                    .Where(p => p.Rental.Id == rental.Id)
                    .ToListAsync();
                var contract = await this.context.Contracts
                    .Where(c => c.RentalId == rental.Id).FirstOrDefaultAsync();

                this.context.Payments.RemoveRange(payments);
                this.context.Contracts.Remove(contract);
                this.context.TransactionRequests.Remove(tr);

                RecurringJob.RemoveIfExists(transactionId);
            }

            // Remove from role Tenan if this is the only Rent which the user terminates.
            if (userRentals.Count() == 1)
            {
                await this.userManager.RemoveFromRoleAsync(user, TenantRole);
            }

            this.context.Rentals.Remove(rental);
            var resultFinal = await this.context.SaveChangesAsync();

            return resultFinal > 0;
        }

        public async Task<IEnumerable<OwnerAllRentalsServiceModel>> GetAllRentalsWithDetailsAsync(string id)
        {
            var rentals = await this.context.Rentals
                .Include(r => r.Home)
                .Include(r => r.Home.City)
                .Include(r => r.Tenant)
                .Include(r => r.Home.Manager)
                .Where(r => r.Home.OwnerId == id)
                .Select(r => new OwnerAllRentalsServiceModel
                {
                    Id = r.Id,
                    Date = r.RentDate.ToString(StandartDateFormat),
                    Duration = r.Duration,

                    Location = string.Format(
                        RentalLocation,
                        r.Home.City.Country.Name,
                        r.Home.City.Name,
                        r.Home.Address),

                    FullName = string.Format(
                        TenantFullName,
                        r.Tenant.FirstName,
                        r.Tenant.LastName),
                    Username = r.Tenant.UserName,

                    ManagerName = string.Format(
                        ManagerFullName,
                        r.Home.Manager.FirstName,
                        r.Home.Manager.LastName),

                    Price = r.Home.Price,
                    HomeCategory = r.Home.Category.ToString(),
                    HomeImage = r.Home.Images.Select(i => i.PictureUrl).FirstOrDefault(),
                })
                .ToListAsync();

            return rentals;
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
                    StartDate = r.RentDate.ToString(StandartDateFormat),
                    Duration = r.Duration,
                    Address = string.Format(DashboardRentalLocation, r.Home.Address),
                    Tenant = string.Format(DashboardRentalFullName, r.Tenant.FirstName, r.Tenant.LastName),
                })
                .ToListAsync();

            return rentalsFromDb;
        }

        public async Task<IEnumerable<OwnerTransactionListOfRentalsServiceModel>>
            GetTransactionRentalsAsync(string userId)
        {
            var rentals = await this.context.Rentals
                .Include(r => r.Home)
                .Include(r => r.Home.City)
                .Where(r => r.Home.OwnerId == userId)
                .Select(r => new OwnerTransactionListOfRentalsServiceModel
                {
                    Id = r.Id,
                    Name = string.Format(DashboardRequestLocation, r.Home.City.Name, r.Home.Address),
                })
                .ToListAsync();

            return rentals;
        }
    }
}
