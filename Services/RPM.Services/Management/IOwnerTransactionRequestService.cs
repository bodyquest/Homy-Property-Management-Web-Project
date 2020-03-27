namespace RPM.Services.Management
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using RPM.Data.Models;
    using RPM.Services.Management.Models;
    using RPM.Web.Areas.Management.Models.TransactionRequests;

    public interface IOwnerTransactionRequestService
    {
        Task<IEnumerable<OwnerAllTransactionRequestsServiceModel>>
            GetAllTransactionRequestsAsync(string userId);

        Task<string> CreateAsync(
            string senderId,
            OwnerTransactionRequestsCreateInputServiceModel model);

        Task<bool> UpdateAsync(TransactionRequest transactionRequest);

        Task<TransactionRequest> FindByIdAsync(string id);
    }
}
