namespace RPM.Services.Common
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using RPM.Services.Common.Models.Profile;

    public interface IPaymentService
    {
        Task<IEnumerable<UserPaymentListServiceModel>> GetUserPaymentsListAsync(string userId);

        Task<bool> AddPaymentRequestToUserAsync(string requestId);
    }
}
