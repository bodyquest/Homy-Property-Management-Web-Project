namespace RPM.Services.Common
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using RPM.Data.Models;
    using RPM.Services.Common.Models.Listing;
    using RPM.Services.Common.Models.Request;

    public interface IRequestService
    {
        Task<bool> CreateRequestAsync(RequestCreateServiceModel model);

        Task<IEnumerable<RequestListServiceModel>> GetRequestsAsync(string id);
    }
}
