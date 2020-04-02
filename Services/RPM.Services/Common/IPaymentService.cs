namespace RPM.Services.Common
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using RPM.Data.Models.Enums;
    using RPM.Services.Common.Models.Payment;
    using RPM.Services.Common.Models.Profile;

    public interface IPaymentService
    {
        Task<IEnumerable<UserPaymentListServiceModel>> GetUserPaymentsListAsync(string userId);

        Task<UserPaymentDetailsServiceModel> GetPaymentDetailsAsync(string paymentId, string userId);

        Task<bool> EditPaymentStatusAsync(string paymentId, string userId, PaymentStatus status, DateTime? date);

        Task<bool> AddPaymentRequestToUserAsync(string requestId);
    }
}