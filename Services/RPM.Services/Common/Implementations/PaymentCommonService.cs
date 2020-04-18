namespace RPM.Services.Common.Implementations
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
    using RPM.Services.Common.Models.Dashboard;
    using RPM.Services.Common.Models.Payment;
    using RPM.Services.Common.Models.Profile;
    using RPM.Services.Management;

    using Stripe.Checkout;

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

        public async Task<IEnumerable<ManagerPaymentListServiceModel>> GetManagerPaymentsListAsync(string userId)
        {
            var payments = await this.context.Homes
                .Include(h => h.City)
                .Where(h => h.ManagerId == userId)
                .SelectMany(h => h.Payments)
                .Select(p => new ManagerPaymentListServiceModel
                {
                    Id = p.Id,
                    Date = p.Date,
                    From = string.Format(RecipientFullName, p.Sender.FirstName, p.Sender.LastName),
                    Reason = p.Reason,
                    Amount = p.Amount,
                    TransactionDate = p.TransactionDate,
                    Status = p.Status,
                    Address = string.Format(
                        HomeLocation, p.Home.City.Name, p.Home.Address),
                })
                .ToListAsync();

            return payments;
        }

        public async Task<UserPaymentDetailsServiceModel> GetPaymentDetailsAsync(string paymentId, string userId)
        {
            var payment = new UserPaymentDetailsServiceModel();

            // FIND if this is the owner who pays
            var paymentFromDb = await this.context.Payments
                .Where(p => p.Id == paymentId)
                .FirstOrDefaultAsync();

            if (paymentFromDb.SenderId == userId)
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
                        PaymentRentalLocation, p.Rental.Home.City.Name, p.Rental.Home.Address),
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

        public async Task<bool> AddPaymentRequestToUserAsync(string userId, string requestId)
        {
            var transactionRequest = await this.ownerTransactionRequestService
                .FindByIdAsync(requestId);

            var senderId = transactionRequest.SenderId;
            var user = await this.userManager.FindByIdAsync(userId);

            Payment payment = new Payment();
            int result = 0;

            if (senderId == userId)
            {
                payment = new Payment
                {
                    RecipientId = transactionRequest.RecipientId,
                    SenderId = userId,
                    Reason = transactionRequest.Reason,
                    Amount = transactionRequest.Amount,
                    Status = PaymentStatus.Waiting,
                    HomeId = transactionRequest.HomeId,
                };

                await this.context.Payments.AddAsync(payment);
                result = await this.context.SaveChangesAsync();

                // var manager = transactionRequest.Recipient;

                // var userManagedHome = await this.context.Homes
                //    .Where(r => r.Id == transactionRequest.HomeId)
                //    .FirstOrDefaultAsync();

                //// TO REMOVE
                // userManagedHome.Payments.Remove(payment);
                // await this.context.SaveChangesAsync();
                //// ...........................................

                // userManagedHome.Payments.Add(payment);
                // result = await this.context.SaveChangesAsync();

                if (result == 0)
                {
                    return false;
                }

                return true;
            }
            else
            {
                payment = new Payment
                {
                    RecipientId = transactionRequest.RecipientId,
                    SenderId = transactionRequest.SenderId,
                    Reason = transactionRequest.Reason,
                    Amount = transactionRequest.Amount,
                    Status = PaymentStatus.Waiting,
                    RentalId = transactionRequest.RentalId,
                };

                await this.context.Payments.AddAsync(payment);
                await this.context.SaveChangesAsync();

                var rentalId = transactionRequest.RentalId;
                var userRental = user.Rentals
                    .Where(r => r.Id == rentalId)
                    .FirstOrDefault();

                userRental.Payments.Add(payment);

                result = await this.context.SaveChangesAsync();

                if (result == 0)
                {
                    return false;
                }

                return true;
            }
        }

        public async Task CreateCheckoutSessionAsync(string sessionId, string paymentId, string toStripeAccountId)
        {
            var session = new StripeCheckoutSession
            {
                Id = sessionId,
                PaymentId = paymentId,
                ToStripeAccountId = toStripeAccountId,
            };

            await this.context.StripeCheckoutSessions.AddAsync(session);
            await this.context.SaveChangesAsync();
        }

        public async Task<bool> MarkPaymentAsCompletedAsync(Session session)
        {
            var sessionId = session.Id;

            var sessionFromDb = await this.context.StripeCheckoutSessions.FindAsync(sessionId);

            if (sessionFromDb == null)
            {
                return false;
            }

            var payment = await this.context.Payments.FindAsync(sessionFromDb.PaymentId);

            payment.Status = PaymentStatus.Complete;
            payment.TransactionDate = DateTime.UtcNow;

            this.context.Payments.Update(payment);
            var result = await this.context.SaveChangesAsync();

            return result > 0;
        }

        public async Task<bool> CompareData(string sessionId)
        {
            bool exists = await this.context.StripeCheckoutSessions.AnyAsync(x => x.Id == sessionId);

            return exists;
        }

        public async Task<string> GetPaymentId(string sessionId)
        {
            var paymentId = await this.context.StripeCheckoutSessions
                .Where(s => s.Id == sessionId)
                .Select(s => s.PaymentId)
                .FirstOrDefaultAsync();

            return paymentId;
        }
    }
}
