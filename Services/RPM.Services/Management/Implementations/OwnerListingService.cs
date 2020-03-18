namespace RPM.Services.Management.Implementations
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using RPM.Data;
    using RPM.Services.Management.Models;

    public class OwnerListingService : IOwnerListingService
    {
        private readonly ApplicationDbContext context;

        public OwnerListingService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<OwnerIndexListingsServiceModel>> GetMyPropertiesAsync()
        {
            throw new NotImplementedException();
        }
    }
}
