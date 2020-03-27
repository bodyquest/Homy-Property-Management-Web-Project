namespace RPM.Services.Common.Implementations
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
    using RPM.Services.Common.Models.Profile;
    using RPM.Services.Management;

    using static RPM.Common.GlobalConstants;

    public class PaymentService : IPaymentService
    {
        private readonly ApplicationDbContext context;
        private readonly IOwnerTransactionRequestService ownerTransactionRequestService;
        private readonly UserManager<User> userManager;
        private readonly IRentalService rentalService;

        public PaymentService(
            ApplicationDbContext context,
            IOwnerTransactionRequestService ownerTransactionRequestService,
            UserManager<User> userManager,
            IRentalService rentalService)
        {
            this.context = context;
            this.ownerTransactionRequestService = ownerTransactionRequestService;
            this.userManager = userManager;
            this.rentalService = rentalService;
        }

        public async Task<IEnumerable<UserPaymentListServiceModel>> GetUserPaymentsListAsync(string userId)
        {
            var payments = await this.context.Rentals
                .Include(r => r.Home)
                .Include(r => r.Home.City)
                .Where(r => r.TenantId == userId)
                .SelectMany(r => r.Payments)
                .Select(p => new UserPaymentListServiceModel
                {
                    Id = p.Id,
                    Date = p.Date,
                    To = string.Format(RecipientFullName, p.Recipient.FirstName, p.Recipient.LastName),
                    Reason = p.Reason,
                    Amount = p.Amount,
                    Status = p.Status,
                    RentalAddress = string.Format(
                        PaymentRentalLocation, p.Rental.Home.City.Name, p.Rental.Home.Address),
                })
                .ToListAsync();

            return payments;
        }

        public async Task<bool> AddPaymentRequestToUserAsync(string requestId)
        {
            var transactionRequest = await this.ownerTransactionRequestService
                .FindByIdAsync(requestId);

            var userId = transactionRequest.SenderId;
            var user = await this.userManager.FindByIdAsync(userId);

            var rentalId = transactionRequest.RentalId;

            var payment = new Payment
            {
                RecipientId = transactionRequest.RecipientId,
                SenderId = transactionRequest.SenderId,
                Reason = transactionRequest.Reason,
                Amount = transactionRequest.Amount,
                Status = PaymentStatus.Waiting,
                RentalId = transactionRequest.RentalId,
            };

            await this.context.Payments.AddAsync(payment);

            var result = await this.context.SaveChangesAsync();

            if (result == 0)
            {
                return false;
            }

            var userRental = user.Rentals.Where(r => r.Id == rentalId).FirstOrDefault();
            userRental.Payments.Add(payment);

            result = await this.context.SaveChangesAsync();

            if (result == 0)
            {
                return false;
            }

            return true;
        }
    }
}
