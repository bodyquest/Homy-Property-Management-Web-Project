namespace RPM.Services.Management.Implementations
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using RPM.Data;
    using RPM.Services.Management.Models;

    public class OwnerRequestService : IOwnerRequestService
    {
        private readonly ApplicationDbContext context;

        public OwnerRequestService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<OwnerIndexRequestsServiceModel>> GetRequestsAsync()
        {
            throw new NotImplementedException();
        }
    }
}
