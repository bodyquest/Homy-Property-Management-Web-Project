namespace RPM.Services.Tests.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using FluentAssertions;
    using Microsoft.EntityFrameworkCore;
    using RPM.Services.Common.Implementations;
    using RPM.Services.Common.Models.Rental;
    using RPM.Services.Tests.Seed;
    using Xunit;

    public class CommonRentalServiceTests : BaseServiceTest
    {
        [Fact] // 1. async Task<IEnumerable<UserRentalListServiceModel>> GetUserRentalsListAsync(string userId)
        public async void GetUserRentalsListAsync_WithGivenUserId_ShouldReturnCollectionOfRentedPropertiesModel()
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

            var service = new RentalService(this.Context);

            // Act
            var result = (await service.GetUserRentalsListAsync(rental1.TenantId)).ToList();
            var expected = await this.Context.Rentals
                .Where(r => r.TenantId == rental1.TenantId)
                .ToListAsync();

            // Assert
            result.Should().AllBeOfType<UserRentalListServiceModel>();
            result.Should().HaveCount(expected.Count());
            result.Should().HaveCount(1, "Because the tenant has 1 rented property.");
        }

        [Fact] // 2. async Task<RentalInfoServiceModel> GetDetailsAsync(int id)
        public async void GetDetailsAsync_WithGivenRenalId_ShouldReturnModelWithDetails()
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

            var service = new RentalService(this.Context);

            // Act
            var result = await service.GetDetailsAsync(rental1.Id);
            var expected = await this.Context.Rentals
                .Where(r => r.Id == rental1.Id)
                .FirstOrDefaultAsync();

            // Assert
            result.Should().BeOfType<RentalInfoServiceModel>();
            result.HomeId.Should().Be(expected.Home.Id);
        }
    }
}
