namespace RPM.Services.Admin.Implementations
{
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using RPM.Data;

    public class AdminRentalService : IAdminRentalService
    {
        private readonly ApplicationDbContext context;

        public AdminRentalService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<int> GetRentalsCount()
        {
            var count = await this.context.Rentals.CountAsync();

            return count;
        }
    }
}
