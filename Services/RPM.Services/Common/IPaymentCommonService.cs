namespace RPM.Services.Common
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using RPM.Data.Models.Enums;
    using RPM.Services.Common.Models.Payment;
    using RPM.Services.Common.Models.Profile;
    using Stripe.Checkout;

    public interface IPaymentCommonService
    {
        Task<IEnumerable<UserPaymentListServiceModel>> GetUserPaymentsListAsync(string userId);

        Task<UserPaymentDetailsServiceModel> GetPaymentDetailsAsync(string paymentId, string userId);

        Task<bool> EditPaymentStatusAsync(string paymentId, string userId, PaymentStatus status, DateTime? date);

        Task<bool> AddPaymentRequestToUserAsync(string requestId);

        Task CreateCheckoutSessionAsync(string sessionId, string paymentId, string toStripeAccountId);

        Task<bool> MarkPaymentAsCompletedAsync(Session session);

        Task<bool> CompareData(string sessionId);

        Task<string> GetPaymentId(string sessionId);
    }
}