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
    using RPM.Services.Common.Models.Payment;
    using RPM.Services.Common.Models.Profile;
    using RPM.Services.Management;

    using static RPM.Common.GlobalConstants;

    public class PaymentCommonService : Common.IPaymentCommonService
    {
        private readonly ApplicationDbContext context;
        private readonly IOwnerTransactionRequestService ownerTransactionRequestService;
        private readonly UserManager<User> userManager;
        private readonly IRentalService rentalService;

        public PaymentCommonService(
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
                    ToStripeAccountId = p.Recipient.StripeConnectedAccountId,
                    Reason = p.Reason,
                    Amount = p.Amount,
                    Status = p.Status,
                    RentalAddress = string.Format(
                        PaymentRentalLocation, p.Rental.Home.City.Name, p.Rental.Home.Address),
                })
                .ToListAsync();

            return payments;
        }

        public async Task<UserPaymentDetailsServiceModel> GetPaymentDetailsAsync(string paymentId, string userId)
        {
            var payment = await this.context.Rentals
                .Where(r => r.TenantId == userId)
                .SelectMany(r => r.Payments)
                .Where(r => r.Id == paymentId)
                .Select(p => new UserPaymentDetailsServiceModel
                {
                    Id = p.Id,
                    Date = p.Date,
                    TransactionDate = p.TransactionDate,
                    To = string.Format(RecipientFullName, p.Recipient.FirstName, p.Recipient.LastName),
                    ToStripeAccountId = p.Recipient.StripeConnectedAccountId,
                    Reason = p.Reason,
                    Amount = p.Amount,
                    Status = p.Status,
                    RentalAddress = string.Format(
                        PaymentRentalLocation, p.Rental.Home.City.Name, p.Rental.Home.Address),
                })
                .FirstOrDefaultAsync();

            return payment;
        }

        public async Task<bool> EditPaymentStatusAsync(string paymentId, string userId, PaymentStatus status, DateTime? date)
        {
            var payment = await this.context.Rentals
                .Where(r => r.TenantId == userId)
                .SelectMany(r => r.Payments)
                .Where(r => r.Id == paymentId)
                .FirstOrDefaultAsync();

            if (!string.IsNullOrWhiteSpace(paymentId)
                && !string.IsNullOrWhiteSpace(userId)
                && Enum.IsDefined(typeof(PaymentStatus), status))
            {
                payment.Status = status;
                payment.TransactionDate = date;

                this.context.Payments.Update(payment);
                var result = await this.context.SaveChangesAsync();

                return result > 0;
            }

            return false;
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
