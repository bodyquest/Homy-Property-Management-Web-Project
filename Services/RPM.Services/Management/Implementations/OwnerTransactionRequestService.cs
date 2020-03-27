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
    using RPM.Web.Areas.Management.Models.TransactionRequests;

    using static RPM.Common.GlobalConstants;

    public class OwnerTransactionRequestService : IOwnerTransactionRequestService
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<User> userManager;

        public OwnerTransactionRequestService(
            ApplicationDbContext context,
            UserManager<User> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        public async Task<IEnumerable<OwnerAllTransactionRequestsServiceModel>>
            GetAllTransactionRequestsAsync(string userId)
        {

            var transactionRequests = await this.context.TransactionRequests
                .Include(t => t.Rental)
                .Include(t => t.Sender)
                .Where(t => t.RecipientId == userId)
                .Select(t => new OwnerAllTransactionRequestsServiceModel
                {
                    Id = t.Id,
                    Date = t.Date.ToString(StandartDateFormat),
                    TenantName = string.Format(TenantFullName, t.Sender.FirstName, t.Sender.LastName),
                    Reason = t.Reason,
                    Amount = t.Amount,
                    IsRecurring = t.IsRecurring,
                    Status = t.Status.ToString(),
                })
                .ToListAsync();

            return transactionRequests;
        }

        public async Task<string> CreateAsync(
            string recipientId, OwnerTransactionRequestsCreateInputServiceModel model)
        {
            var rental = await this.context.Rentals
                .Include(r => r.Tenant)
                .Include(r => r.Home)
                .Where(r => r.Id == model.RentalId)
                .FirstOrDefaultAsync();

            var senderId = rental.TenantId;
            var amount = rental.Home.Price;

            var transactionRequestFromDb = rental.TransactionRequests.FirstOrDefault(t => t.IsRecurring == true);

            if (transactionRequestFromDb != null)
            {
                return null;
            }

            var transactionRequest = new TransactionRequest
            {
                Reason = model.Reason,
                Amount = amount,
                RecurringSchedule = model.RecurringSchedule,
                Status = TransactionRequestStatus.Scheduled,
                IsRecurring = model.IsRecurring,
                RentalId = model.RentalId,
                SenderId = senderId,
                RecipientId = recipientId,
            };

            await this.context.TransactionRequests.AddAsync(transactionRequest);
            var result = await this.context.SaveChangesAsync();

            if (result == 0)
            {
                return null;
            }

            var id = transactionRequest.Id;

            return id;
        }

        public async Task<bool> UpdateAsync(TransactionRequest transactionRequest)
        {
            this.context.TransactionRequests.Update(transactionRequest);
            var result = await this.context.SaveChangesAsync();

            if (result > 0)
            {
                return true;
            }

            return false;
        }

        public async Task<TransactionRequest> FindByIdAsync(string id)
        {
            var transactionRequest = await this.context.TransactionRequests
                .Include(t => t.Rental)
                .Include(t => t.Rental.Home)
                .Where(t => t.Id == id)
                .FirstOrDefaultAsync();

            return transactionRequest;
        }
    }
}
