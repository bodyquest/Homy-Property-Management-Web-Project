namespace RPM.Services.Management.Implementations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using RPM.Data;
    using RPM.Services.Common.Models.Payment;
    using RPM.Services.Management.Models;
    using static RPM.Common.GlobalConstants;

    public class OwnerPaymentService : IOwnerPaymentService
    {
        private readonly ApplicationDbContext context;

        public OwnerPaymentService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<OwnerAllPaymentsServiceModel>> AllPayments(string userId)
        {
            var homePayments = await this.context.Payments
                .Where(p => p.Home.OwnerId == userId && p.Home.Manager != null)
                .Select(p => new OwnerAllPaymentsServiceModel
                {
                    Id = p.Id,
                    Date = p.Date.ToString(StandartDateFormat),
                    TransactionDate = p.TransactionDate != null ? p.TransactionDate.Value.ToString(DateFormatWithTime) : "n/a",
                    HomeOwnerName = p.Home.Owner.FirstName,
                    ManagerName = string.Format(ManagerFullName, p.Home.Manager.FirstName, p.Home.Manager.LastName),
                    Reason = p.Reason,
                    Amount = p.Amount,
                    Status = p.Status,
                })
                .ToListAsync();

            var rentalPayments = await this.context.Payments
                .Include(p => p.Recipient)
                .Include(p => p.Sender)
                .Include(p => p.Home)
                .Where(p => p.Rental.Home.OwnerId == userId)
                .Select(p => new OwnerAllPaymentsServiceModel
                {
                    Id = p.Id,
                    Date = p.Date.ToString(StandartDateFormat),
                    TransactionDate = p.TransactionDate != null ? p.TransactionDate.Value.ToString(DateFormatWithTime) : "n/a",
                    RentalHomeOnwerName = p.Rental.Home.Owner.FirstName,
                    TenantName = string.Format(TenantFullName, p.Sender.FirstName, p.Sender.LastName),
                    Reason = p.Reason,
                    Amount = p.Amount,
                    Status = p.Status,
                })
                .ToListAsync();

            IEnumerable<OwnerAllPaymentsServiceModel> model =
                homePayments.Concat(rentalPayments);

            return model;
        }

        // CreateSession Method Calls this Service
        public async Task<UserPaymentDetailsServiceModel> GetPaymentDetailsAsync(string paymentId, string userId)
        {
            var payment = new UserPaymentDetailsServiceModel();

            // FIND if this is the owner who pays
            var paymentFromDb = await this.context.Payments
                .Include(p => p.Home)
                .Where(p => p.Id == paymentId)
                .FirstOrDefaultAsync();

            if (paymentFromDb.HomeId != null && paymentFromDb.Home.OwnerId == userId)
            {
                payment = await this.context.Payments
                .Where(p => p.Id == paymentId && p.Home.OwnerId == userId)
                .Select(p => new UserPaymentDetailsServiceModel
                {
                    Id = p.Id,
                    Date = p.Date,
                    TransactionDate = p.TransactionDate,
                    To = string.Format(RecipientFullName, p.Recipient.FirstName, p.Recipient.LastName),
                    RecipientHasStripeAccount = !string.IsNullOrWhiteSpace(p.Recipient.StripeConnectedAccountId),
                    ToStripeAccountId = p.Recipient.StripeConnectedAccountId,
                    Reason = p.Reason,
                    Amount = p.Amount,
                    Status = p.Status,
                    Address = string.Format(
                        HomeLocation, p.Home.City.Name, p.Home.Address, p.Home.Category.ToString()),
                })
                .FirstOrDefaultAsync();
            }
            else
            {
                payment = await this.context.Rentals
                .Where(r => r.TenantId == userId)
                .SelectMany(r => r.Payments)
                .Where(p => p.Id == paymentId)
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
                    Address = string.Format(
                        PaymentRentalLocation, p.Rental.Home.City.Name, p.Rental.Home.Address),
                })
                .FirstOrDefaultAsync();
            }

            return payment;
        }
    }
}
