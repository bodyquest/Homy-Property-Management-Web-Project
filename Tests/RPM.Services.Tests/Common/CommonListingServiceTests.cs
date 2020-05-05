namespace RPM.Services.Tests.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using FluentAssertions;
    using RPM.Data.Models.Enums;
    using RPM.Services.Common.Implementations;
    using RPM.Services.Common.Models.Home;
    using RPM.Services.Common.Models.Listing;
    using RPM.Services.Tests.Seed;
    using Xunit;
    using static RPM.Common.GlobalConstants;

    public class CommonListingServiceTests : BaseServiceTest
    {
        #region Service Methods
        /*
         * // 1. public async Task<IEnumerable<PropertyListServiceModel>> GetPropertiesAsync()
         *
         * // 2. public async Task<IEnumerable<ManagerDashboardPropertiesServiceModel>> GetManagedPropertiesAsync(string Id)
         *
         * // 3. public async Task<IEnumerable<PropertyListServiceModel>> GetAllByCategoryAsync(HomeCategory category)
         *
         * // 4. public async Task<PropertyDetailsServiceModel> GetDetailsAsync(string id)
         *
         * // 5. public async Task<ManagedHomeInfoServiceModel> GetManagedDetailsAsync(string id)
         *
         * // 6. public async Task<PropertyCountServiceModel> GetPropertyCountByCategoryAsync(string category)
         *
         * // 7. public async Task<IEnumerable<PropertyListServiceModel>> FindAsync(string searchText)
         *
         * // 8. public async Task<IEnumerable<PropertyListServiceModel>> GetAllByStatusAsync(string status)
         *
         *  // 9. private async Task<IEnumerable<PropertyListServiceModel>> GetAllByCategoryAsync(HomeCategory category, HomeStatus managedStatus)
         *
         * // 10. private async Task<IEnumerable<PropertyListServiceModel>> GetByStatusAsync(HomeStatus status)
         *
         * // 11. private async Task<PropertyCountServiceModel> GetByCategoryAsync(HomeStatus managed, HomeCategory category)
         */
        #endregion

        [Fact] // 1. public async Task<IEnumerable<PropertyListServiceModel>> GetPropertiesAsync()
        public async void GetPropertiesAsync_ShouldReturnAllListings_ThatAreNotManaged()
        {
            // Arrange
            var ownerId = Guid.NewGuid().ToString();
            var country = CountryCreator.Create();
            var city = CityCreator.Create(country.Id);

            var home1 = HomeCreator.CreateManagedHome(ownerId, city.Id);
            var home2 = HomeCreator.CreateManagedHome(ownerId, city.Id);
            var home3 = HomeCreator.CreateAny(city.Id);

            await this.Context.Countries.AddAsync(country);
            await this.Context.Cities.AddAsync(city);
            await this.Context.Homes.AddRangeAsync(home1, home2, home3);
            await this.Context.SaveChangesAsync();

            var service = new ListingService(this.Context);

            // Act
            var result = (await service.GetPropertiesAsync()).ToList();
            var expectedCount = this.Context.Homes
                .Where(h => h.Status != HomeStatus.Managed)
                .Count();

            // Assert
            result.Should().AllBeOfType<PropertyListServiceModel>();
            result.Should().HaveCount(expectedCount);
            result.Should().HaveCount(1, "Because only one home is not with status [Managed]");
        }

        [Fact] // 2. public async Task<IEnumerable<ManagerDashboardPropertiesServiceModel>> GetManagedPropertiesAsync(string Id)
        public async void GetManagedPropertiesAsync_ShouldReturnAllManagedProperties()
        {
            // Arrange
            var ownerId = Guid.NewGuid().ToString();
            var country = CountryCreator.Create();
            var city = CityCreator.Create(country.Id);

            var home1 = HomeCreator.CreateManagedHome(ownerId, city.Id);
            var home4 = HomeCreator.CreateAny(city.Id);
            var home5 = HomeCreator.CreateAny(city.Id);

            home1.Owner = UserCreator.Create("Kanalin", "Tsolov", "tsola", "kanalin@prasmail.com");

            await this.Context.Countries.AddAsync(country);
            await this.Context.Cities.AddAsync(city);
            await this.Context.Homes.AddRangeAsync(home1, home4, home5);
            await this.Context.SaveChangesAsync();

            var service = new ListingService(this.Context);

            // Act
            var result = (await service.GetManagedPropertiesAsync(home1.ManagerId)).ToList();
            var expectedCount = this.Context.Homes
                .Where(h => h.Status == HomeStatus.Managed && h.ManagerId == home1.ManagerId)
                .Count();

            // Assert
            result.Should().AllBeOfType<ManagerDashboardPropertiesServiceModel>();
            result.Should().HaveCount(expectedCount);
            result.Should().HaveCount(1, "Because there is 1 managed home by this manager");
        }

        [Fact] // 3. public async Task<IEnumerable<PropertyListServiceModel>> GetAllByCategoryAsync(HomeCategory category)
        public async void GetAllByCategoryAsync_WithGivenCategory_ShouldReturnAll()
        {
            // Arrange
            var ownerId = Guid.NewGuid().ToString();
            var country = CountryCreator.Create();
            var city = CityCreator.Create(country.Id);

            var home4 = HomeCreator.CreateAny(city.Id);
            var home5 = HomeCreator.CreateAny(city.Id);

            await this.Context.Countries.AddAsync(country);
            await this.Context.Cities.AddAsync(city);
            await this.Context.Homes.AddRangeAsync(home4, home5);
            await this.Context.SaveChangesAsync();

            var service = new ListingService(this.Context);

            // Act
            var result = (await service.GetAllByCategoryAsync(HomeCategory.House)).ToList();
            var expectedCount = this.Context.Homes
                .Where(h => h.Category == HomeCategory.House)
                .Count();

            // Assert
            result.Should().AllBeOfType<PropertyListServiceModel>();
            result.Should().HaveCount(expectedCount);
            result.Should().HaveCount(2, "Because there 2 homes with category [House]");
        }

        [Fact] // 4. public async Task<PropertyDetailsServiceModel> GetDetailsAsync(string id)
        public async void GetDetailsAsync_WithGivenListingId_ShouldReturnModelWithDetails()
        {
            // Arrange
            var ownerId = Guid.NewGuid().ToString();
            var country = CountryCreator.Create();
            var city = CityCreator.Create(country.Id);

            var home4 = HomeCreator.CreateAny(city.Id);
            var home5 = HomeCreator.CreateAny(city.Id);

            await this.Context.Countries.AddAsync(country);
            await this.Context.Cities.AddAsync(city);
            await this.Context.Homes.AddRangeAsync(home4, home5);
            await this.Context.SaveChangesAsync();

            var service = new ListingService(this.Context);

            // Act
            var result = await service.GetDetailsAsync(home4.Id);
            var expectedCount = this.Context.Homes
                .Where(h => h.Id == home4.Id)
                .Count();

            // Assert
            result.Should().BeOfType<PropertyDetailsServiceModel>();
            result.Description.Should().Match(home4.Description);
        }

        [Fact] // 5. public async Task<ManagedHomeInfoServiceModel> GetManagedDetailsAsync(string id)
        public async void GetManagedDetailsAsync_WithGivenListingId_ShouldReturnManagedListingModel()
        {
            // Arrange
            var ownerId = Guid.NewGuid().ToString();
            var country = CountryCreator.Create();
            var city = CityCreator.Create(country.Id);

            var home1 = HomeCreator.CreateManagedHome(ownerId, city.Id);
            var home4 = HomeCreator.CreateAny(city.Id);
            var home5 = HomeCreator.CreateAny(city.Id);
            home1.Owner = UserCreator.Create("Kanalin", "Tsolov", "tsola", "kanalin@prasmail.com");

            await this.Context.Countries.AddAsync(country);
            await this.Context.Cities.AddAsync(city);
            await this.Context.Homes.AddRangeAsync(home1, home4, home5);
            await this.Context.SaveChangesAsync();

            var service = new ListingService(this.Context);

            // Act
            var result = await service.GetManagedDetailsAsync(home1.Id);
            var expectedCount = this.Context.Homes
                .Where(h => h.Id == home1.Id)
                .Count();

            // Assert
            result.Should().BeOfType<ManagedHomeInfoServiceModel>();
            result.Owner.Should().Match(string.Format(OwnerFullName, home1.Owner.FirstName, home1.Owner.LastName));
        }

        [Fact] // 6. public async Task<PropertyCountServiceModel> GetPropertyCountByCategoryAsync(string category)
        public async void GetPropertyCountByCategoryAsync_WithGivenCategoryString_ShouldReturnCountOfListings()
        {
            // Arrange
            var country = CountryCreator.Create();
            var city = CityCreator.Create(country.Id);

            var home3 = HomeCreator.CreateAny(city.Id);
            var home4 = HomeCreator.CreateAny(city.Id);
            var home5 = HomeCreator.CreateAny(city.Id);
            home3.Category = HomeCategory.Room;

            await this.Context.Countries.AddAsync(country);
            await this.Context.Cities.AddAsync(city);
            await this.Context.Homes.AddRangeAsync(home3, home4, home5);
            await this.Context.SaveChangesAsync();

            var service = new ListingService(this.Context);

            // Act
            var result = await service.GetPropertyCountByCategoryAsync(HomeCategory.House.ToString());
            var expectedCount = this.Context.Homes
                .Where(h => h.Category == HomeCategory.House)
                .Count();

            // Assert
            result.Should().BeOfType<PropertyCountServiceModel>();
            result.Count.Should().Equals(expectedCount);
            result.Count.Should().Be(2, "Because there 2 homes with category [House]");
        }

        [Fact] // 7. public async Task<IEnumerable<PropertyListServiceModel>> FindAsync(string searchText)
        public async void FindAsync_WithGivenSearchStringForCity_ShouldReturnListModel()
        {
            // Arrange
            var country = CountryCreator.Create();
            var city = CityCreator.Create(country.Id);
            var city2 = CityCreator.Create(country.Id);

            var home1 = HomeCreator.CreateAny(city2.Id);
            home1.City = city2;
            var home2 = HomeCreator.CreateAny(city.Id);
            var home3 = HomeCreator.CreateAny(city.Id);
            var home4 = HomeCreator.CreateAny(city.Id);
            var home5 = HomeCreator.CreateAny(city.Id);

            await this.Context.Countries.AddAsync(country);
            await this.Context.Cities.AddRangeAsync(city, city2);
            await this.Context.Homes.AddRangeAsync(home1, home2, home3, home4, home5);
            await this.Context.SaveChangesAsync();

            string search = city2.Name;
            var service = new ListingService(this.Context);

            // Act
            var result = (await service.FindAsync(search)).ToList();
            var expectedCount = this.Context.Homes
                .Where(h => h.City.Name == search)
                .Count();

            // Assert
            result.Should().AllBeOfType<PropertyListServiceModel>();
            result.Should().HaveCount(expectedCount);
            result.Should().HaveCount(1, "Because there is 1 home from the searched city");
        }

        [Fact] // 8. public async Task<IEnumerable<PropertyListServiceModel>> GetAllByStatusAsync(string status)
        public async void GetAllByStatusAsync_WithGivenStatusString_ShouldReturnAll()
        {
            // Arrange
            var country = CountryCreator.Create();
            var city = CityCreator.Create(country.Id);

            var home1 = HomeCreator.CreateAny(city.Id);
            var home2 = HomeCreator.CreateAny(city.Id);
            var home3 = HomeCreator.CreateAny(city.Id);
            var home4 = HomeCreator.CreateAny(city.Id);
            var home5 = HomeCreator.CreateAny(city.Id);

            home4.Status = HomeStatus.ToManage;
            home5.Status = HomeStatus.Managed;

            await this.Context.Countries.AddAsync(country);
            await this.Context.Cities.AddAsync(city);
            await this.Context.Homes.AddRangeAsync(home1, home2, home3, home4, home5);
            await this.Context.SaveChangesAsync();

            var service = new ListingService(this.Context);

            // Act
            var result = (await service.GetAllByStatusAsync(HomeStatus.ToRent.ToString())).ToList();
            var expectedCount = this.Context.Homes
                .Where(h => h.Status == HomeStatus.ToRent)
                .Count();

            // Assert
            result.Should().AllBeOfType<PropertyListServiceModel>();
            result.Should().HaveCount(expectedCount);
            result.Should().HaveCount(3, "Because there 3 homes with status [To Rent]");
        }

        [Fact] // 9. private async Task<IEnumerable<PropertyListServiceModel>> GetAllByCategoryAsync(HomeCategory category, HomeStatus managedStatus)
        public async void GetAllByCategoryAsync_()
        {
            // Arrange
            var country = CountryCreator.Create();
            var city = CityCreator.Create(country.Id);

            var home1 = HomeCreator.CreateAny(city.Id);
            var home2 = HomeCreator.CreateAny(city.Id);
            var home3 = HomeCreator.CreateAny(city.Id);
            var home4 = HomeCreator.CreateAny(city.Id);
            var home5 = HomeCreator.CreateAny(city.Id);

            home4.Status = HomeStatus.Rented;
            home4.Category = HomeCategory.Room;
            home5.Status = HomeStatus.Rented;
            home5.Category = HomeCategory.Room;

            await this.Context.Countries.AddAsync(country);
            await this.Context.Cities.AddAsync(city);
            await this.Context.Homes.AddRangeAsync(home1, home2, home3, home4, home5);
            await this.Context.SaveChangesAsync();

            var service = new ListingService(this.Context);

            // Act
            Type type = typeof(ListingService);
            var getAllByCategoryAsync = Activator.CreateInstance(type, this.Context);
            MethodInfo method = type.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
            .Where(x => x.Name == "GetAllByCategoryAsync" && x.IsPrivate)
            .First();

            var result = (Task<IEnumerable<PropertyListServiceModel>>)method
                .Invoke(getAllByCategoryAsync, new object[] { HomeCategory.Room, HomeStatus.Managed });

            var resultList = (await result).ToList();

            var expectedCount = this.Context.Homes
                .Where(h => h.Status != HomeStatus.ToRent && h.Category == HomeCategory.Room)
                .Count();

            // Assert
            resultList.Should().AllBeOfType<PropertyListServiceModel>();
            resultList.Should().HaveCount(expectedCount);
            resultList.Should().HaveCount(2, "Because there 2 homes with status not [Managed] and in category [Room]");
        }

        [Fact] // 10. PRIVATE async Task<IEnumerable<PropertyListServiceModel>> GetByStatusAsync(HomeStatus status)
        public async void GetByStatusAsync_WithGivenCategoryEnum_ShouldReturnAll()
        {
            // Arrange
            var country = CountryCreator.Create();
            var city = CityCreator.Create(country.Id);

            var home1 = HomeCreator.CreateAny(city.Id);
            var home2 = HomeCreator.CreateAny(city.Id);
            var home3 = HomeCreator.CreateAny(city.Id);
            var home4 = HomeCreator.CreateAny(city.Id);
            var home5 = HomeCreator.CreateAny(city.Id);

            home4.Status = HomeStatus.ToManage;
            home5.Status = HomeStatus.Managed;

            await this.Context.Countries.AddAsync(country);
            await this.Context.Cities.AddAsync(city);
            await this.Context.Homes.AddRangeAsync(home1, home2, home3, home4, home5);
            await this.Context.SaveChangesAsync();

            var service = new ListingService(this.Context);

           // Act
            Type type = typeof(ListingService);
            var getByStatusAsync = Activator.CreateInstance(type, this.Context);
            MethodInfo method = type.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
            .Where(x => x.Name == "GetByStatusAsync" && x.IsPrivate)
            .First();

            var result = (Task<IEnumerable<PropertyListServiceModel>>)method
                .Invoke(getByStatusAsync, new object[] { HomeStatus.ToRent });

            var resultList = (await result).ToList();

            var expectedCount = this.Context.Homes
                .Where(h => h.Status == HomeStatus.ToRent)
                .Count();

            // Assert
            resultList.Should().AllBeOfType<PropertyListServiceModel>();
            resultList.Should().HaveCount(expectedCount);
            resultList.Should().HaveCount(3, "Because there 3 homes with status [To Rent]");
        }

        [Fact] // 11. PRIVATE async Task<PropertyCountServiceModel> GetByCategoryAsync(HomeStatus managed, HomeCategory category)
        public async void GetByStatusAsync_WithGivenCategoryEnumAndManagedStatusEnum_ShouldReturnModel()
        {
            // Arrange
            var country = CountryCreator.Create();
            var city = CityCreator.Create(country.Id);

            var home1 = HomeCreator.CreateAny(city.Id);
            var home2 = HomeCreator.CreateAny(city.Id);
            var home3 = HomeCreator.CreateAny(city.Id);
            var home4 = HomeCreator.CreateAny(city.Id);
            var home5 = HomeCreator.CreateAny(city.Id);

            home4.Status = HomeStatus.Rented;
            home4.Category = HomeCategory.Room;
            home5.Status = HomeStatus.Rented;
            home5.Category = HomeCategory.Room;

            await this.Context.Countries.AddAsync(country);
            await this.Context.Cities.AddAsync(city);
            await this.Context.Homes.AddRangeAsync(home1, home2, home3, home4, home5);
            await this.Context.SaveChangesAsync();

            var service = new ListingService(this.Context);

            // Act
            Type type = typeof(ListingService);
            var getByCategoryAsync = Activator.CreateInstance(type, this.Context);
            MethodInfo method = type.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
            .Where(x => x.Name == "GetByCategoryAsync" && x.IsPrivate)
            .First();

            var result = (Task<PropertyCountServiceModel>)method
                .Invoke(getByCategoryAsync, new object[] { HomeStatus.Managed, HomeCategory.Room });

            var resultFinal = await result;

            var expectedCount = this.Context.Homes
                .Where(h => h.Status != HomeStatus.Managed && h.Category == HomeCategory.Room)
                .Count();

            // Assert
            resultFinal.Should().BeOfType<PropertyCountServiceModel>();
            resultFinal.Count.Should().Equals(expectedCount);
            resultFinal.Count.Should().Be(2, "Because there 2 homes with status which is not [Managed] and category [Room]");
        }
    }
}
