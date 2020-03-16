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
    using static RPM.Common.Extensions.StringExtensionMethod;

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

        public async Task<IEnumerable<PropertyListServiceModel>> GetAllByStatusAsync(string status)
        {
            var toRent = (HomeStatus)Enum.Parse(typeof(HomeStatus), ToRent, true);
            var toManage = (HomeStatus)Enum.Parse(typeof(HomeStatus), ToManage, true);

            if (status == ToRent)
            {
                return await this.GetByStatusAsync(toRent);
            }
            else
            {
                return await this.GetByStatusAsync(toManage);
            }
        }

        public async Task<PropertyCountServiceModel> GetPropertyCountByCategoryAsync(string category)
        {
            var managed = (HomeStatus)Enum.Parse(typeof(HomeStatus), Managed, true);

            if (category == House)
            {
                var house = (HomeCategory)Enum.Parse(typeof(HomeCategory), House, true);

                return await this.GetByCategoryAsync(managed, house);
            }
            else if (category == Apartment)
            {
                var apartment = (HomeCategory)Enum.Parse(typeof(HomeCategory), Apartment, true);

                return await this.GetByCategoryAsync(managed, apartment);
            }
            else
            {
                var room = (HomeCategory)Enum.Parse(typeof(HomeCategory), Room, true);

                return await this.GetByCategoryAsync(managed, room);
            }
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

        private async Task<PropertyCountServiceModel> GetByCategoryAsync(HomeStatus managed, HomeCategory category)
        {
            var count = await this.context.Homes
                .Where(h => h.Status != managed && h.Category == category)
               .CountAsync();

            var exampleImage = await this.context.Homes
                .Include(h => h.Images)
                .Where(h => h.Status != managed && h.Category == category)
                .Select(x => x.Images.Select(i => i.PictureUrl).FirstOrDefault())
                .FirstOrDefaultAsync();

            var categoryUpper = category.ToString().FirstCharToUpper();

            var model = new PropertyCountServiceModel
            {
                CategoryName = category.ToString().FirstCharToUpper() + "s",
                Count = count,
                ExampleImage = exampleImage,
            };

            return model;
        }

        private async Task<IEnumerable<PropertyListServiceModel>> GetByStatusAsync(HomeStatus status)
        {
            var model = await this.context.Homes
                .Include(x => x.City)
                .Include(x => x.City.Country)
                .Include(h => h.Images)
                .Where(h => h.Status == status)
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
