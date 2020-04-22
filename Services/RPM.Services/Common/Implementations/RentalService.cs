namespace RPM.Services.Common.Implementations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using RPM.Data;
    using RPM.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using RPM.Services.Common.Models.Rental;

    using static RPM.Common.GlobalConstants;

    public class RentalService : IRentalService
    {
        private readonly ApplicationDbContext context;

        public RentalService(
            ApplicationDbContext context
           )
        {
            this.context = context;
        }

        public async Task<IEnumerable<UserRentalListServiceModel>> GetUserRentalsListAsync(string userId)
        {
            var rentals = await this.context.Rentals
                .Include(r => r.Home)
                .Where(r => r.TenantId == userId)
                .Select(r => new UserRentalListServiceModel
                {
                    Id = r.Id,
                    Date = r.RentDate.ToString(StandartDateFormat),
                    OwnerFullName = string.Format(OwnerFullName, r.Home.Owner.FirstName, r.Home.Owner.LastName),
                    Location = string.Format(RentalLocation, r.Home.Address, r.Home.City.Name, r.Home.City.Country.Name),
                    Price = r.Home.Price,
                })
                .ToListAsync();

            return rentals;
        }

        public async Task<RentalInfoServiceModel> GetDetailsAsync(int id)
        {
            var model = await this.context.Rentals
                .Include(r => r.Home)
                .Include(r => r.Home.City)
                .Where(r => r.Id == id)
                .Select(r => new RentalInfoServiceModel
                {
                    Id = r.Id,
                    HomeId = r.Home.Id,
                    RentalDate = r.RentDate.ToString(StandartDateFormat),
                    Price = r.Home.Price,
                    Location = string.Format(HomeLocation, r.Home.Address, r.Home.City.Name, r.Home.City.Country.Name),
                    Owner = string.Format(
                        OwnerFullName, r.Home.Owner.FirstName, r.Home.Owner.LastName),
                    Image = r.Home.Images.Select(i => i.PictureUrl).FirstOrDefault(),
                })
                .FirstOrDefaultAsync();

            return model;
        }
    }
}
