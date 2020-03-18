namespace RPM.Services.Management.Implementations
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using RPM.Data;
    using RPM.Services.Management.Models;

    public class OwnerRentalService : IOwnerRentalService
    {
        private readonly ApplicationDbContext context;

        public OwnerRentalService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<OwnerIndexRentalServiceModel>> GetRentalsAsync()
        {
            throw new NotImplementedException();
        }
    }
}
