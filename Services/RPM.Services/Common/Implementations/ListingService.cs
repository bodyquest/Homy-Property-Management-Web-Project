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
    using RPM.Services.Common.Models.Listing;
    using RPM.Services.Common.Models.Rental;

    using static RPM.Common.Extensions.StringExtensionMethod;
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

        public async Task<IEnumerable<ManagerDashboardPropertiesServiceModel>> GetManagedPropertiesAsync(string Id)
        {
            var rentals = await this.context.Rentals
              .Where(r => r.Home.ManagerId == Id)
              .Select(r => new ManagerDashboardRentalServiceModel
              {
                  Id = r.Id,
                  HomeId = r.HomeId,
                  TenantFullName = string.Format(TenantFullName, r.Tenant.FirstName, r.Tenant.LastName),
              })
              .ToListAsync();

            var properties = await this.context.Homes
                .Include(p => p.City)
                .Include(p => p.Owner)
                .Where(p => p.ManagerId == Id)
                .Select(p => new ManagerDashboardPropertiesServiceModel
                {
                    Id = p.Id,
                    OwnerName = string.Format(OwnerFullName, p.Owner.FirstName, p.Owner.LastName),
                    City = p.City.Name,
                    Address = p.Address,
                    Status = p.Status,
                    Category = p.Category,
                })
                .ToListAsync();

            foreach (var house in properties)
            {
                foreach (var rent in rentals)
                {
                    if (rent.HomeId == house.Id)
                    {
                        house.TenantName =
                            rent.TenantFullName;
                    }
                }
            }

            return properties;
        }

        public async Task<IEnumerable<PropertyListServiceModel>> GetAllByCategoryAsync(HomeCategory category)
        {
            var managedStatus = HomeStatus.Managed;

            if (category == HomeCategory.House)
            {
                return await this.GetAllByCategoryAsync(category, managedStatus);
            }
            else if (category == HomeCategory.Apartment)
            {
                return await this.GetAllByCategoryAsync(category, managedStatus);
            }
            else
            {
                return await this.GetAllByCategoryAsync(category, managedStatus);
            }
        }

        public async Task<PropertyDetailsServiceModel> GetDetailsAsync(string id)
        {
            var model = await this.context.Homes
                .Where(h => h.Id == id)
                .Include(x => x.City)
                .Include(x => x.City.Country)
                .Include(h => h.Images)
                .Select(x => new PropertyDetailsServiceModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    OwnerName = x.Owner.FirstName,
                    City = x.City.Name,
                    Address = x.Address,
                    Country = x.City.Country.Name,
                    Description = x.Description,
                    Price = x.Price,
                    Status = x.Status,
                    Category = x.Category,
                    Image = x.Images.Select(i => i.PictureUrl).FirstOrDefault(),
                })
                .FirstOrDefaultAsync();

            return model;
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

        private async Task<IEnumerable<PropertyListServiceModel>> GetAllByCategoryAsync(HomeCategory category, HomeStatus managedStatus)
        {
            var model = await this.context.Homes
                            .Include(x => x.City)
                            .Include(x => x.City.Country)
                            .Include(h => h.Images)
                            .Where(h => h.Category == category && h.Status != managedStatus)
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

        private async Task<PropertyCountServiceModel> GetByCategoryAsync(HomeStatus managed, HomeCategory category)
        {
            var exampleImage = string.Empty;

            var count = await this.context.Homes
                .Where(h => h.Status != managed && h.Category == category)
               .CountAsync();

            if (category == HomeCategory.Apartment)
            {
                exampleImage = ApartmentImage;
            }
            else if (category == HomeCategory.House)
            {
                exampleImage = HouseImage;
            }
            else if (category == HomeCategory.Room)
            {
                exampleImage = RoomImage;
            }

            var categoryUpper = category.ToString().FirstCharToUpper();

            var model = new PropertyCountServiceModel
            {
                CategoryName = category.ToString().FirstCharToUpper() + "s",
                Count = count,
                ExampleImage = exampleImage,
            };

            return model;
        }
    }
}
