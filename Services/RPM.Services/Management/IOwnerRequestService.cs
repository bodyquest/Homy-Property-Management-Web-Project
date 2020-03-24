namespace RPM.Services.Management
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using RPM.Data.Models;
    using RPM.Services.Management.Models;

    public interface IOwnerRequestService
    {
        Task<IEnumerable<OwnerIndexRequestsServiceModel>> GetRequestsAsync(string id);

        Task<Request> ApproveRentRequestAsync(string id);

        Task<byte[]> GetFileAsync(string requestId);

        Task<OwnerRequestInfoServiceModel> GetRequestInfoAsync(string requestId);

        Task<OwnerRequestDetailsServiceModel> GetRequestDetailsAsync(string requestId);
    }
}
