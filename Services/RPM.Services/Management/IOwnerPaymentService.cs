namespace RPM.Services.Management
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using RPM.Services.Common.Models.Payment;
    using RPM.Services.Management.Models;

    public interface IPaymentService
    {
        Task<IEnumerable<OwnerAllPaymentsServiceModel>> AllPayments(string userId);

        Task<UserPaymentDetailsServiceModel> GetPaymentDetailsAsync(string paymentId, string userId);
    }
}
