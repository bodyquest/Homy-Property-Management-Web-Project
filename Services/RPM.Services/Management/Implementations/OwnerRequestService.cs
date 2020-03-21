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

    using static RPM.Common.GlobalConstants;

    public class OwnerRequestService : IOwnerRequestService
    {
        private readonly ApplicationDbContext context;

        public OwnerRequestService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<OwnerIndexRequestsServiceModel>> GetRequestsAsync(string id)
        {
            var requestsFromDb = await this.context.Requests
                .Where(requests => requests.Home.OwnerId == id)
                .OrderBy(r => r.Date)
                .Select(r => new OwnerIndexRequestsServiceModel
                {
                    Id = r.Id,
                    Date = r.Date.ToString("dd/MM/yyyy h:mm tt"),
                    Type = r.Type.ToString(),
                    FullName = string.Format(DashboardRequestFullName, r.User.FirstName, r.User.LastName),
                    Location = string.Format(DashboardRequestLocation, r.Home.City.Name, r.Home.Address),
                })
                .ToListAsync();

            return requestsFromDb;
        }
    }
}
