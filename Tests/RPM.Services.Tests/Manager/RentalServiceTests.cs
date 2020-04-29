namespace RPM.Services.Tests.Manager
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using RPM.Data.Models;
    using RPM.Services.Management;
    using RPM.Services.Management.Implementations;
    using RPM.Services.Management.Models;
    using RPM.Services.Tests.Seed;
    using Xunit;

    using static RPM.Common.GlobalConstants;

    public class RentalServiceTests : BaseServiceTest
    {
        #region Service Methods
         /* // async Task<bool> StartRent(string id, byte[] fileContent)
         *
         * public async Task<bool> StopRentAsync(string id)
         *
         * // async Task<IEnumerable<OwnerAllRentalsServiceModel>> GetAllRentalsWithDetailsAsync(string id)
         *
         * // async Task<IEnumerable<OwnerIndexRentalServiceModel>> GetRentalsAsync(string userId)
         *
         * // async Task<IEnumerable<OwnerTransactionListOfRentalsServiceModel>> GetTransactionRentalsAsync(string userId)
         */
        #endregion

        [Fact] // async Task<bool> StartRent(string id, byte[] fileContent)
        public async void StartRent_ForGivenOwnerId_ShouldSuccessfullyInitiateRentAndReturnTrue()
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

            var approvedRequest = RequestCreator.CreateRentApproved(home, request.User, request.Id);

            var user = request.User;
            this.UserManager
                .Setup(u => u.FindByIdAsync(user.Id))
                .Returns(Task.FromResult(user));
            await this.UserManager.Object
                .AddToRoleAsync(user, "Tenant");

            var requestService = new Mock<IOwnerRequestService>();
            requestService
                .Setup(x => x.ApproveRequestAsync(request.Id))
                .Returns(Task.FromResult(approvedRequest));

            var listingService = new Mock<IOwnerListingService>();
            listingService
                .Setup(x => x.ChangeHomeStatusAsync(approvedRequest))
                .Returns(Task.FromResult(approvedRequest.HomeId));

            var rental = new Rental
            {
                Id = 1,
                RentDate = DateTime.UtcNow,
                HomeId = home.Id,
                TenantId = user.Id,
            };

            var rentalServiceMock = new Mock<IOwnerRentalService>();
            rentalServiceMock.Setup(y => y.CreateRental(home.Id, user.Id))
                .Returns(Task.FromResult(rental));

            var contractService = new Mock<IOwnerContractService>();
            contractService
                .Setup(x => x.CreateRentalContractAsync(new byte[1024], request.User, rental))
                .Returns(Task.FromResult(true));

            var service = new OwnerRentalService(this.Context, requestService.Object, listingService.Object, contractService.Object, this.UserManager.Object);

            // Act
            var result = await service.StartRent(request.Id, new byte[1024]);
            var rentalInfo = await this.Context.Rentals.Where(r => r.Home.Id == home.Id).FirstOrDefaultAsync();
            var changedUser = await this.Context.Users.Where(u => u.Id == user.Id).FirstOrDefaultAsync();

            // Assert
            result.Should().BeTrue();
            rentalInfo.TenantId.Should().Equals(user.Id);
            changedUser.Rentals.Count().Should().Be(1);
        }

        [Fact] // async Task<IEnumerable<OwnerAllRentalsServiceModel>> GetAllRentalsWithDetailsAsync(string id)
        public async void GetAllRentalsWithDetailsAsync_ForGivenOwnerId_ShouldReturnModelOfAllRentedHomes()
        {
            // Arrange
            var ownerId = Guid.NewGuid().ToString();
            var anotherOwnerId = Guid.NewGuid().ToString();
            var country = CountryCreator.Create();
            var city = CityCreator.Create(country.Id);

            var home1 = HomeCreator.CreateOwnerHome(ownerId, city.Id);
            var home2 = HomeCreator.CreateManagedHome(ownerId, city.Id);
            var home3 = HomeCreator.CreateManagedHome(ownerId, city.Id);
            var home4 = HomeCreator.CreateManagedHome(anotherOwnerId, city.Id);

            var user1 = UserCreator.Create("Debelin", "Butov", "but4eto", "but@prasmail.com");
            var user2 = UserCreator.Create("Shunko", "Shpekov", "shpeka", "shpek@prasmail.com");
            var user3 = UserCreator.Create("Suzdurma", "Saturov", "satura", "satur@prasmail.com");

            int id1 = 1;
            int id2 = 2;
            int id3 = 3;
            int id4 = 4;

            var contract1 = ContractCreator.CreateRentalContract(id1);
            var contract2 = ContractCreator.CreateRentalContract(id2);
            var contract3 = ContractCreator.CreateRentalContract(id3);
            var contract4 = ContractCreator.CreateRentalContract(id4);

            var rental1 = RentalCreator.Create(id1, country, city, user1, home1, contract1);
            var rental2 = RentalCreator.Create(id2, country, city, user2, home2, contract2);
            var rental3 = RentalCreator.Create(id3, country, city, user3, home3, contract3);
            var rental4 = RentalCreator.Create(id4, country, city, user3, home4, contract4);

            await this.Context.Countries.AddAsync(country);
            await this.Context.Cities.AddAsync(city);
            await this.Context.Homes.AddRangeAsync(home1, home2, home3, home4);
            await this.Context.Users.AddRangeAsync(user1, user2, user3);
            await this.Context.Rentals.AddRangeAsync(rental1, rental2, rental3, rental4);
            await this.Context.Contracts.AddRangeAsync(contract1, contract2, contract3, contract4);

            await this.Context.SaveChangesAsync();

            var service = new OwnerRentalService(this.Context, null, null, null, null);

            // Act
            var result = (await service.GetAllRentalsWithDetailsAsync(ownerId)).ToList();
            var expectedCount = this.Context.Rentals
                .Where(r => r.Home.OwnerId == ownerId)
                .Count();

            // Assert
            result.Should().AllBeOfType<OwnerAllRentalsServiceModel>();
            result.Should().HaveCount(expectedCount);
            result.Should().HaveCount(3, "because I think I've put 3 rentals associated with this owner");
            result[0].Should().BeOfType<OwnerAllRentalsServiceModel>()
                .Which.Date
                .Should().Be(rental1.RentDate.ToString(StandartDateFormat));
            result[0].Should().BeOfType<OwnerAllRentalsServiceModel>()
                .Which.Duration
                .Should().Be(rental1.Duration);
            result[0].Should().BeOfType<OwnerAllRentalsServiceModel>()
                .Which.FullName
                .Should().Be(string.Format(TenantFullName, rental1.Tenant.FirstName, rental1.Tenant.LastName));
            result[0].Should().BeOfType<OwnerAllRentalsServiceModel>()
                .Which.Username
                .Should().Be("but4eto");
            result[0].Should().BeOfType<OwnerAllRentalsServiceModel>()
                .Which.Price
                .Should().Be(rental1.Home.Price);
        }

        [Fact] // async Task<IEnumerable<OwnerIndexRentalServiceModel>> GetRentalsAsync(string userId)
        public async void GetRentalsAsync_ForGivenOwnerId_ShouldReturnModelOfAllRentedHomes()
        {
            // Arrange
            var ownerId = Guid.NewGuid().ToString();
            var anotherOwnerId = Guid.NewGuid().ToString();
            var country = CountryCreator.Create();
            var city = CityCreator.Create(country.Id);

            var home1 = HomeCreator.CreateOwnerHome(ownerId, city.Id);
            var home2 = HomeCreator.CreateManagedHome(ownerId, city.Id);
            var home3 = HomeCreator.CreateManagedHome(ownerId, city.Id);
            var home4 = HomeCreator.CreateManagedHome(anotherOwnerId, city.Id);

            var user1 = UserCreator.Create("Debelin", "Butov", "but4eto", "but@prasmail.com");
            var user2 = UserCreator.Create("Shunko", "Shpekov", "shpeka", "shpek@prasmail.com");
            var user3 = UserCreator.Create("Suzdurma", "Saturov", "satura", "satur@prasmail.com");

            int id1 = 1;
            int id2 = 2;
            int id3 = 3;
            int id4 = 4;

            var contract1 = ContractCreator.CreateRentalContract(id1);
            var contract2 = ContractCreator.CreateRentalContract(id2);
            var contract3 = ContractCreator.CreateRentalContract(id3);
            var contract4 = ContractCreator.CreateRentalContract(id4);

            var rental1 = RentalCreator.Create(id1, country, city, user1, home1, contract1);
            var rental2 = RentalCreator.Create(id2, country, city, user2, home2, contract2);
            var rental3 = RentalCreator.Create(id3, country, city, user3, home3, contract3);
            var rental4 = RentalCreator.Create(id4, country, city, user3, home4, contract4);

            await this.Context.Countries.AddAsync(country);
            await this.Context.Cities.AddAsync(city);
            await this.Context.Homes.AddRangeAsync(home1, home2, home3, home4);
            await this.Context.Users.AddRangeAsync(user1, user2, user3);
            await this.Context.Rentals.AddRangeAsync(rental1, rental2, rental3, rental4);
            await this.Context.Contracts.AddRangeAsync(contract1, contract2, contract3, contract4);

            await this.Context.SaveChangesAsync();

            var service = new OwnerRentalService(this.Context, null, null, null, null);

            // Act
            var result = (await service.GetRentalsAsync(ownerId)).ToList();
            var expectedCount = this.Context.Rentals
                .Where(r => r.Home.OwnerId == ownerId)
                .Count();

            // Assert
            result.Should().AllBeOfType<OwnerIndexRentalServiceModel>();
            result.Should().HaveCount(expectedCount);
            result.Should().HaveCount(3, "because I've put 3 rentals associated with this owner");
            result[0].Should().BeOfType<OwnerIndexRentalServiceModel>()
                .Which.StartDate
                .Should().Be(rental1.RentDate.ToString(StandartDateFormat));
            result[0].Should().BeOfType<OwnerIndexRentalServiceModel>()
                .Which.Duration
                .Should().Be(rental1.Duration);
            result[0].Should().BeOfType<OwnerIndexRentalServiceModel>()
                .Which.Tenant
                .Should().Be(string.Format(TenantFullName, rental1.Tenant.FirstName, rental1.Tenant.LastName));
        }

        [Fact] // async Task<IEnumerable<OwnerTransactionListOfRentalsServiceModel>> GetTransactionRentalsAsync(string userId)
        public async void GetTransactionRentalsAsync_ForGivenOwnerId_ShouldReturnListOfRentalsForSelectList()
        {
            // Arrange
            var ownerId = Guid.NewGuid().ToString();
            var anotherOwnerId = Guid.NewGuid().ToString();
            var country = CountryCreator.Create();
            var city = CityCreator.Create(country.Id);

            var home1 = HomeCreator.CreateOwnerHome(ownerId, city.Id);
            var home2 = HomeCreator.CreateManagedHome(ownerId, city.Id);
            var home3 = HomeCreator.CreateManagedHome(ownerId, city.Id);
            var home4 = HomeCreator.CreateManagedHome(anotherOwnerId, city.Id);

            var user1 = UserCreator.Create("Debelin", "Butov", "but4eto", "but@prasmail.com");
            var user2 = UserCreator.Create("Shunko", "Shpekov", "shpeka", "shpek@prasmail.com");
            var user3 = UserCreator.Create("Suzdurma", "Saturov", "satura", "satur@prasmail.com");

            int id1 = 1;
            int id2 = 2;
            int id3 = 3;
            int id4 = 4;

            var contract1 = ContractCreator.CreateRentalContract(id1);
            var contract2 = ContractCreator.CreateRentalContract(id2);
            var contract3 = ContractCreator.CreateRentalContract(id3);
            var contract4 = ContractCreator.CreateRentalContract(id4);

            var rental1 = RentalCreator.Create(id1, country, city, user1, home1, contract1);
            var rental2 = RentalCreator.Create(id2, country, city, user2, home2, contract2);
            var rental3 = RentalCreator.Create(id3, country, city, user3, home3, contract3);
            var rental4 = RentalCreator.Create(id4, country, city, user3, home4, contract4);

            await this.Context.Countries.AddAsync(country);
            await this.Context.Cities.AddAsync(city);
            await this.Context.Homes.AddRangeAsync(home1, home2, home3, home4);
            await this.Context.Users.AddRangeAsync(user1, user2, user3);
            await this.Context.Rentals.AddRangeAsync(rental1, rental2, rental3, rental4);
            await this.Context.Contracts.AddRangeAsync(contract1, contract2, contract3, contract4);

            await this.Context.SaveChangesAsync();

            var service = new OwnerRentalService(this.Context, null, null, null, null);

            // Act
            var result = (await service.GetTransactionRentalsAsync(ownerId)).ToList();
            var expectedCount = this.Context.Rentals
                .Where(r => r.Home.OwnerId == ownerId)
                .Count();

            // Assert
            result.Should().AllBeOfType<OwnerTransactionListOfRentalsServiceModel>();
            result.Should().HaveCount(expectedCount);
            result.Should().HaveCount(3);
        }
    }
}
