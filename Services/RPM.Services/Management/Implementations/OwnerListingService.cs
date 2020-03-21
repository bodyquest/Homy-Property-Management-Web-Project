namespace RPM.Services.Management.Implementations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using RPM.Data;
    using RPM.Services.Management.Models;

    public class OwnerListingService : IOwnerListingService
    {
        private readonly ApplicationDbContext context;

        public OwnerListingService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<OwnerIndexListingsServiceModel>> GetMyPropertiesAsync(string id)
        {
            var homes = await this.context.Homes
                .Where(homes => homes.OwnerId == id)
                .Select(h => new OwnerIndexListingsServiceModel
                {
                    Id = h.Id,
                    City = h.City.Name,
                    Address = h.Address,
                    Category = h.Category.ToString(),
                    Status = h.Status.ToString(),
                })
                .ToListAsync();

            return homes;
        }
    }
}
