namespace RPM.Services.Common.Implementations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using RPM.Data;
    using RPM.Data.Models.Enums;
    using RPM.Services.Common.Models.Home;

    using static RPM.Common.GlobalConstants;

    public class ListingService : IListingService
    {
        private readonly ApplicationDbContext context;

        public ListingService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<PropertyListServiceModel>> GetPropertiesAsync()
        {
            var managed = (HomeStatus)Enum.Parse(typeof(HomeStatus), Managed, true);

            var model = await this.context.Homes
                .Include(x => x.City)
                .Include(x => x.City.Country)
                .Include(h => h.Images)
                .Where(h => h.Status != managed)
                .Select(x => new PropertyListServiceModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    City = x.City.Name,
                    Country = x.City.Country.Name,
                    Price = x.Price,
                    Status = x.Status,
                    Category = x.Category,
                    Image = x.Images.Select(i => i.PictureUrl).FirstOrDefault(),
                })
                .ToListAsync();

            return model;
        }

        public async Task<IEnumerable<PropertyListServiceModel>> FindAsync(string searchText)
        {
            searchText = searchText ?? string.Empty;

            var model = await this.context.Homes
                .Where(h => h.City.Name.ToLower().Contains(searchText.ToLower()))
                .Select(x => new PropertyListServiceModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    City = x.City.Name,
                    Country = x.City.Country.Name,
                    Price = x.Price,
                    Status = x.Status,
                    Category = x.Category,
                    Image = x.Images.Select(i => i.PictureUrl).FirstOrDefault(),
                })
                .ToListAsync();

            return model;
        }
    }
}
