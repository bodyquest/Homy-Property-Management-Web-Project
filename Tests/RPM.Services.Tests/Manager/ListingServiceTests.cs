namespace RPM.Services.Tests.Manager
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using RPM.Data.Models.Enums;
    using RPM.Services.Management;
    using RPM.Services.Management.Implementations;
    using RPM.Services.Management.Models;
    using RPM.Services.Tests.Seed;
    using Xunit;

    public class ListingServiceTests : BaseServiceTest
    {
        #region Service Methods
        /*
         * // async Task<IEnumerable<OwnerIndexListingsServiceModel>> GetMyPropertiesAsync(string id)
         *
         * // async Task<bool> CreateListingAsync(OwnerCreateListingServiceModel model)
         *
         * // async Task<bool> DeleteAsync(string id)
         *
         * // async Task<bool> EditListingAsync(OwnerEditListingServiceModel model)
         *
         * // async Task<OwnerListingFullDetailsServiceModel> GetDetailsAsync(string userId, string id)
         *
         * async Task<OwnerListingFullDetailsServiceModel> GetEditModelAsync(string userId, string id)
         *
         * async Task<IEnumerable<OwnerPropertyWithDetailsServiceModel>> GetMyPropertiesWithDetailsAsync(string userId)
         *
         * // async Task<string> ChangeHomeStatusAsync(Request request)
         *
         * // async Task<bool> StartHomeManage(string id, byte[] fileContent)
         *
         * async Task<bool> StopHomeManageAsync(string id)
         *
         * // async Task<IEnumerable<OwnerTransactionListOfManagedHomesServiceModel>>
           GetManagedHomesAsync(string userId)
         *
         * // async Task<bool> IsHomeDeletable(string id)
         *
         */
        #endregion

        [Fact] // async Task<IEnumerable<OwnerIndexListingsServiceModel>> GetMyPropertiesAsync(string id)
        public async void GetMyPropertiesAsync_ForGivenOwnerId_ShouldReturnCorrectlyAllOwnerListings()
        {
            // Arrange
            var ownerId = Guid.NewGuid().ToString();
            var country = CountryCreator.Create();
            var city = CityCreator.Create(country.Id);

            var home1 = HomeCreator.CreateOwnerHome(ownerId, city.Id);
            var home2 = HomeCreator.CreateOwnerHome(ownerId, city.Id);
            var home3 = HomeCreator.CreateOwnerHome(ownerId, city.Id);

            await this.Context.Countries.AddAsync(country);
            await this.Context.Cities.AddAsync(city);
            await this.Context.Homes.AddRangeAsync(home1, home2, home3);
            await this.Context.SaveChangesAsync();

            var service = new OwnerListingService(this.Context, null, null, null);

            // Act
            var result = (await service.GetMyPropertiesAsync(ownerId)).ToList();
            var expectedCount = this.Context.Homes
                .Where(h => h.OwnerId == ownerId)
                .Count();

            // Assert
            result.Should().AllBeOfType<OwnerIndexListingsServiceModel>();
            result.Should().HaveCount(expectedCount);
        }

        [Fact] // async Task<bool> CreateListingAsync(OwnerCreateListingServiceModel model)
        public async Task CreateListingAsync_WithGivenInputModel_ShouldCreateOwnerListingAndReturnTrue()
        {
            // Arrange
            const string modelName = "New house close to the beach.";
            const string modelDescription = "That we can tuck in our children at night and know that they are fed and clothed and safe from harm. I wanted to be part of something larger. And yet, it has only been in the last couple of weeks that the discussion of race in this campaign has taken a particularly divisive turn.";
            const string modelAddress = "10, G.M. Dimitrov, 1700";

            var owner = UserCreator.Create("Prasemir", "Butonoskov", "prasio", "prasi@prasmail.com");
            var country = CountryCreator.Create();
            var city = CityCreator.Create(country.Id);
            var image = ImageCreator.CreateForModel();

            await this.Context.Countries.AddAsync(country);
            await this.Context.Cities.AddAsync(city);
            await this.Context.Users.AddAsync(owner);
            await this.Context.CloudImages.AddAsync(image);
            await this.Context.SaveChangesAsync();

            var model = new OwnerCreateListingServiceModel
            {
                Name = modelName,
                Description = modelDescription,
                Address = modelAddress,
                CityId = city.Id,
                Price = 1000,
                Status = HomeStatus.ToRent,
                Category = HomeCategory.House,
                Owner = owner,
                Image = image,
            };

            var service = new OwnerListingService(this.Context, null, null, null);

            // Act
            var result = await service.CreateListingAsync(model);
            var savedEntry = await this.Context.Homes.Where(h => h.OwnerId == owner.Id).FirstOrDefaultAsync();

            var expected = true;

            // Assert
            result.Should().Be(true);
            result.Should().Equals(expected);

            savedEntry.Should().NotBeNull();
            savedEntry.Name.Should().Match(modelName);
            savedEntry.Description.Should().Match(modelDescription);
            savedEntry.OwnerId.Should().Match(owner.Id);
            savedEntry.Images.Any(i => i.PictureUrl == image.PictureUrl);
        }

        [Fact] // async Task<bool> DeleteAsync(string id)
        public async Task DeleteAsync_WithGivenListingId_ShouldRemoveListingAndReturnTrue()
        {
            // Arrange
            var ownerId = Guid.NewGuid().ToString();
            var country = CountryCreator.Create();
            var city = CityCreator.Create(country.Id);

            var home1 = HomeCreator.CreateOwnerHome(ownerId, city.Id);

            await this.Context.Countries.AddAsync(country);
            await this.Context.Cities.AddAsync(city);
            await this.Context.Homes.AddAsync(home1);
            await this.Context.SaveChangesAsync();

            var service = new OwnerListingService(this.Context, null, null, null);

            // Act
            var result = await service.DeleteAsync(home1.Id);
            var expected = true;

            // Assert
            result.Should().Be(true);
            result.Should().Equals(expected);
        }

        [Fact] // async Task<bool> EditListingAsync(OwnerEditListingServiceModel model)
        public async Task EditHomeStatusAsync_WithGivenRequestObject_ShouldChangeStatusAndReturnString()
        {
            // Arrange
            string newName = "New house on the block";
            string newDescription = "Well maintained house close to the beach";
            HomeStatus newStatus = HomeStatus.Rented;
            HomeCategory newCategory = HomeCategory.Room;

            var country = CountryCreator.Create();
            var city = CityCreator.Create(country.Id);
            var image = ImageCreator.CreateForModel();
            var home = HomeCreator.CreateAny(city.Id);

            await this.Context.Countries.AddAsync(country);
            await this.Context.Cities.AddAsync(city);
            await this.Context.Homes.AddAsync(home);
            await this.Context.CloudImages.AddAsync(image);
            await this.Context.SaveChangesAsync();

            var model = new OwnerEditListingServiceModel
            {
                Id = home.Id,
                Name = newName,
                Description = newDescription,
                Price = 1000m,
                Status = newStatus,
                Category = newCategory,
                Image = image,
            };

            var service = new OwnerListingService(this.Context, null, null, null);

            // Act
            var savedEntry = await this.Context.Homes.Where(h => h.Id == home.Id).FirstOrDefaultAsync();
            var result = await service.EditListingAsync(model);
            var expected = true;

            // Assert
            result.Should().Be(true);
            result.Should().Equals(expected);

            savedEntry.Should().NotBeNull();
            savedEntry.Id.Should().Be(model.Id);
            savedEntry.Name.Should().Match(model.Name);
            savedEntry.Description.Should().Match(model.Description);
            savedEntry.Price.Should().Be(model.Price);
            savedEntry.Status.Should().Be(model.Status);
            savedEntry.Category.Should().Be(model.Category);
            savedEntry.Images.Select(i => i.PictureUrl).FirstOrDefault()
                .Should()
                .Match(model.Image.PictureUrl);
        }

        [Fact] // async Task<OwnerListingFullDetailsServiceModel> GetDetailsAsync(string userId, string id)
        public async Task GetyDetailsAsync_WithGivenOwnerIdAndHomeId_ShouldReturnFullDetailsModel()
        {
            // Arrange
            var id = 1;
            var country = CountryCreator.Create();
            var city = CityCreator.Create(country.Id);
            var tenant = UserCreator.Create("Shunko", "Svinski", "shunkata", "svin@prasmail.com");
            var owner = UserCreator.Create("Suzdurma", "Saturov", "satrura", "satur@prasmail.com");
            var home = HomeCreator.CreateOwnerHome(owner.Id, city.Id);
            var contract = ContractCreator.Create();

            var rental = RentalCreator.Create(id, country, city, tenant, home, contract);

            await this.Context.Countries.AddAsync(country);
            await this.Context.Cities.AddAsync(city);
            await this.Context.Users.AddAsync(tenant);
            await this.Context.Homes.AddAsync(home);
            await this.Context.Contracts.AddAsync(contract);
            await this.Context.Rentals.AddAsync(rental);
            await this.Context.SaveChangesAsync();

            var rentalInfo = new OwnerRentalInfoServiceModel
            {
                RentalDate = rental.RentDate.ToString(),
                Duration = rental.Duration,
                ManagerFullName = null,
                TenantFullName = string.Format(
                        "{0} {1}", rental.Tenant.FirstName, rental.Tenant.LastName),
            };

            var model = new OwnerListingFullDetailsServiceModel
            {
                Id = home.Id,
                Name = home.Name,
                City = home.City.Name,
                Country = home.City.Country.Name,
                Address = home.Address,
                Description = home.Description,
                Price = home.Price,
                Status = home.Status,
                Category = home.Category,
                Image = home.Images.Select(i => i.PictureUrl).FirstOrDefault(),
                RentalInfo = rentalInfo,
            };

            var service = new OwnerListingService(this.Context, null, null, null);

            // Act
            var savedEntry = await this.Context.Homes.Where(h => h.OwnerId == owner.Id).FirstOrDefaultAsync();
            var savedRental = await this.Context.Rentals.Where(r => r.Home.Id == home.Id).FirstOrDefaultAsync();
            var result = await service.GetDetailsAsync(owner.Id, home.Id);

            // Assert
            result.Should().BeOfType<OwnerListingFullDetailsServiceModel>();
            result.RentalInfo.RentalDate.Should().Match(savedRental.RentDate.ToString("dd/MM/yyyy"));

            savedEntry.Should().NotBeNull();
            savedEntry.Id.Should().Be(home.Id);
            savedEntry.Name.Should().Match(model.Name);
            savedEntry.Description.Should().Match(model.Description);
        }

        [Fact] // async Task<string> ChangeHomeStatusAsync(Request request)
        public async Task ChangeHomeStatusAsync_WithGivenRequestObject_ShouldReturnHomeIdWhenSuccessful()
        {
            // Arrange
            var country = CountryCreator.Create();
            var city = CityCreator.Create(country.Id);
            var home = HomeCreator.CreateAny(city.Id);
            var image = ImageCreator.CreateForModel();

            await this.Context.Countries.AddAsync(country);
            await this.Context.Cities.AddAsync(city);
            await this.Context.Homes.AddAsync(home);
            await this.Context.CloudImages.AddAsync(image);

            var request = RequestCreator.Create(home);
            await this.Context.Requests.AddAsync(request);

            await this.Context.SaveChangesAsync();

            var service = new OwnerListingService(this.Context, null, null, null);

            // Act
            var result = await service.ChangeHomeStatusAsync(request);
            var homeFromDb = await this.Context.Homes.Where(h => h.Id == home.Id).FirstOrDefaultAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<string>();
            result.Should().Match(home.Id);
            result.Should().Match(homeFromDb.Id);
            homeFromDb.Status.Should().NotBe(HomeStatus.ToRent);
            homeFromDb.Status.Should().Be(HomeStatus.Rented);
        }

        [Fact] // async Task<IEnumerable<OwnerTransactionListOfManagedHomesServiceModel>>GetManagedHomesAsync(string userId)
        public async Task GetManagedHomesAsync_WithGivenOwnerId_ShouldReturnManagedHomesModel()
        {
            // Arrange
            var ownerId = Guid.NewGuid().ToString();
            var country = CountryCreator.Create();
            var city = CityCreator.Create(country.Id);

            var home1 = HomeCreator.CreateOwnerHome(ownerId, city.Id);
            var home2 = HomeCreator.CreateManagedHome(ownerId, city.Id);
            var home3 = HomeCreator.CreateManagedHome(ownerId, city.Id);

            await this.Context.Countries.AddAsync(country);
            await this.Context.Cities.AddAsync(city);
            await this.Context.Homes.AddRangeAsync(home1, home2, home3);
            await this.Context.SaveChangesAsync();

            var service = new OwnerListingService(this.Context, null, null, null);

            // Act
            var result = (await service.GetManagedHomesAsync(ownerId)).ToList();
            var expectedCount = this.Context.Homes
                .Where(h => h.OwnerId == ownerId && h.Manager != null)
                .Count();

            // Assert
            result.Should().AllBeOfType<OwnerTransactionListOfManagedHomesServiceModel>();
            result.Should().HaveCount(expectedCount);
        }

        [Fact] // async Task<bool> StartHomeManage(string id, byte[] fileContent)
        public async Task StartHomeManage_WithGivenRequestIdAndDocumentFile_ShouldInitiateManagementContract()
        {
            // Arrange
            var country = CountryCreator.Create();
            var city = CityCreator.Create(country.Id);
            var home = HomeCreator.CreateAny(city.Id);
            var image = ImageCreator.CreateForModel();

            await this.Context.Countries.AddAsync(country);
            await this.Context.Cities.AddAsync(city);
            await this.Context.Homes.AddAsync(home);
            await this.Context.CloudImages.AddAsync(image);

            var request = RequestCreator.Create(home);
            await this.Context.Requests.AddAsync(request);

            await this.Context.SaveChangesAsync();

            var approvedRequest = RequestCreator.CreateManageApproved(home, request.User, request.Id);

            var user = request.User;
            this.UserManager
                .Setup(u => u.FindByIdAsync(user.Id))
                .Returns(Task.FromResult(user));
            await this.UserManager.Object
                .AddToRoleAsync(user, "Manager");

            var requestService = new Mock<IOwnerRequestService>();
            requestService.Setup(x => x.ApproveRequestAsync(request.Id))
                .Returns(Task.FromResult(approvedRequest));

            var contractService = new Mock<IOwnerContractService>();
            contractService.Setup(x => x.CreateManageContractAsync(new byte[1024], request.User))
                .Returns(Task.FromResult(true));

            var service = new OwnerListingService(this.Context, this.UserManager.Object, requestService.Object, contractService.Object);

            // Act
            var result = await service.StartHomeManage(request.Id, new byte[1024]);
            var changedHomeInfo = await this.Context.Homes.Where(h => h.Id == home.Id).FirstOrDefaultAsync();
            var changedUser = await this.Context.Users.Where(u => u.Id == user.Id).FirstOrDefaultAsync();

            // Assert
            result.Should().BeTrue();
            changedHomeInfo.ManagerId.Should().Equals(user.Id);
            changedUser.ManagedHomes.Count().Should().Be(1);
        }

        [Fact] // async Task<bool> IsHomeDeletable(string id)
        public async Task IsHomeDeletable_WithGivenHomeId_ShouldConfirmIf_HomeHasTenantManagerOrNone()
        {
            // Arrange
            var id = 1;
            var ownerId = Guid.NewGuid().ToString();
            var country = CountryCreator.Create();
            var city = CityCreator.Create(country.Id);

            var home1 = HomeCreator.CreateOwnerHome(ownerId, city.Id);
            var homeManaged = HomeCreator.CreateManagedHome(ownerId, city.Id);
            var homeRented = HomeCreator.CreateOwnerHome(ownerId, city.Id);
            var tenant = UserCreator.Create("Shunko", "Svinski", "shunkata", "svin@prasmail.com");

            var contract = ContractCreator.Create();

            var rental = RentalCreator.Create(id, country, city, tenant, homeRented, contract);

            await this.Context.Countries.AddAsync(country);
            await this.Context.Cities.AddAsync(city);
            await this.Context.Homes.AddRangeAsync(home1, homeManaged, homeRented);
            await this.Context.Users.AddAsync(tenant);
            await this.Context.Contracts.AddAsync(contract);
            await this.Context.Rentals.AddAsync(rental);
            await this.Context.SaveChangesAsync();

            var service = new OwnerListingService(this.Context, null, null, null);

            // Act
            var result1 = await service.IsHomeDeletable(home1.Id);
            var resultManaged = await service.IsHomeDeletable(homeManaged.Id);
            var resultRented = await service.IsHomeDeletable(homeRented.Id);

            // Assert
            result1.Should().Be(true);
            resultManaged.Should().Be(false);
            resultRented.Should().Be(false);
        }
    }
}
