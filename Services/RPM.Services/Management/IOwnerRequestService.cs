namespace RPM.Services.Management
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    using RPM.Services.Management.Models;

    public interface IOwnerRequestService
    {
        Task<IEnumerable<OwnerIndexRequestsServiceModel>> GetRequestsAsync(string id);
    }
}
