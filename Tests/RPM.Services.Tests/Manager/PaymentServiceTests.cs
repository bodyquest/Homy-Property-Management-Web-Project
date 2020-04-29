namespace RPM.Services.Tests.Manager
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using FluentAssertions;
    using Microsoft.EntityFrameworkCore;
    using RPM.Services.Common.Models.Payment;
    using RPM.Services.Management;
    using RPM.Services.Management.Implementations;
    using RPM.Services.Management.Models;
    using RPM.Services.Tests.Seed;
    using Xunit;

    public class PaymentServiceTests : BaseServiceTest
    {
        #region Service Methods
        /*
         * // async Task<IEnumerable<OwnerAllPaymentsServiceModel>> AllPayments(string userId)
         *
         * // async Task<UserPaymentDetailsServiceModel> GetPaymentDetailsAsync(string paymentId, string userId)
         */
        #endregion

        [Fact] // async Task<IEnumerable<OwnerAllPaymentsServiceModel>> AllPayments(string userId)
        public async void AllPayments_ForGivenOwnerId_ShouldReturnAllRelatedPayments()
        {
            // Arrange
            var ownerId = Guid.NewGuid().ToString();
            var anotherOwnerId = Guid.NewGuid().ToString();
            var country = CountryCreator.Create();
            var city = CityCreator.Create(country.Id);

            var home1 = HomeCreator.CreateAny(city.Id); // rented
            var home2 = HomeCreator.CreateManagedHome(home1.Owner.Id, city.Id); // managed
            var home3 = HomeCreator.CreateManagedHome(home1.Owner.Id, city.Id); // managed & rented
            var home4 = HomeCreator.CreateAny(city.Id); // another owner with tenant

            var tenant1 = UserCreator.Create("Debelin", "Butov", "but4eto", "but@prasmail.com");
            var tenant3 = UserCreator.Create("Shunko", "Shpekov", "shpeka", "shpek@prasmail.com");
            var tenant4 = UserCreator.Create("Suzdurma", "Saturov", "satura", "satur@prasmail.com");

            int id1 = 1;
            int id3 = 3;
            int id4 = 4;

            var contract1 = ContractCreator.CreateRentalContract(id1);
            var contract3 = ContractCreator.CreateRentalContract(id3);
            var contract4 = ContractCreator.CreateRentalContract(id4);

            var rental1 = RentalCreator.Create(id1, country, city, tenant1, home1, contract1);
            var rental3 = RentalCreator.Create(id3, country, city, tenant3, home3, contract3);
            var rental4 = RentalCreator.Create(id4, country, city, tenant4, home4, contract4);

            var payment1 = PaymentCreator.CreateForTenant(home1.Owner, tenant1.Id, rental1.Id);
            var payment2 = PaymentCreator.CreateForManager(home1.Owner.Id, home2.Manager.Id, home2.Id);
            var payment3 = PaymentCreator.CreateForManager(home1.Owner.Id, home3.Manager.Id, home3.Id);
            var payment4 = PaymentCreator.CreateForTenant(home1.Owner, tenant3.Id, rental3.Id);
            var paymentAnother5 = PaymentCreator.CreateForTenant(home4.Owner, tenant4.Id, rental4.Id);

            await this.Context.Countries.AddAsync(country);
            await this.Context.Cities.AddAsync(city);
            await this.Context.Homes.AddRangeAsync(home1, home2, home3, home4);
            await this.Context.Users.AddRangeAsync(tenant1, tenant3, tenant4);
            await this.Context.Rentals.AddRangeAsync(rental1, rental3, rental4);
            await this.Context.Payments.AddRangeAsync(payment1, payment2, payment3, payment4, paymentAnother5);
            await this.Context.Contracts.AddRangeAsync(contract1, contract3, contract4);

            await this.Context.SaveChangesAsync();

            var service = new OwnerPaymentService(this.Context);

            // Act
            var result = (await service.AllPayments(home1.Owner.Id)).ToList();
            var expectedCount = this.Context.Payments
                .Where(p => p.Home.OwnerId == home1.Owner.Id || p.Rental.Home.OwnerId == home1.Owner.Id)
                .Count();

            // Assert
            result.Should().AllBeOfType<OwnerAllPaymentsServiceModel>();
            result.Should().HaveCount(expectedCount);
            result.Should().HaveCount(4, "because there are 3 properties where one has tenant + manager, which makes 4");
        }

        [Fact] // async Task<UserPaymentDetailsServiceModel> GetPaymentDetailsAsync(string paymentId, string userId)
        public async void GetPaymentDetailsAsync_ForGivenOwnerIdAndPaymentId_ShouldReturnPaymentDetailsModel()
        {
            // Arrange
            var country = CountryCreator.Create();
            var city = CityCreator.Create(country.Id);
            var manager = UserCreator.Create("Georgi", "Butov", "joro", "joro@prasmail.com");

            var home1 = HomeCreator.CreateAny(city.Id); // rented
            var home2 = HomeCreator.CreateManagedHome(home1.Owner.Id, city.Id); // managed
            var home4 = HomeCreator.CreateAny(city.Id); // another owner with tenant

            var tenant1 = UserCreator.Create("Debelin", "Dignibutov", "but4eto", "but@prasmail.com");
            var tenant4 = UserCreator.Create("Suzdurma", "Saturov", "satura", "satur@prasmail.com");

            int id1 = 1;
            int id4 = 4;

            var contract1 = ContractCreator.CreateRentalContract(id1);
            var contract4 = ContractCreator.CreateRentalContract(id4);

            var rental1 = RentalCreator.Create(id1, country, city, tenant1, home1, contract1);
            var rental4 = RentalCreator.Create(id4, country, city, tenant4, home4, contract4);

            var payment1 = PaymentCreator.CreateForTenant(home1.Owner, tenant1.Id, rental1.Id);
            var payment2 = PaymentCreator.CreateForManager(home1.Owner.Id, home2.Manager.Id, home2.Id);
            var paymentAnother5 = PaymentCreator.CreateForTenant(home4.Owner, tenant4.Id, rental4.Id);

            await this.Context.Countries.AddAsync(country);
            await this.Context.Cities.AddAsync(city);
            await this.Context.Homes.AddRangeAsync(home1, home2, home4);
            await this.Context.Users.AddRangeAsync(tenant1, tenant4);
            await this.Context.Rentals.AddRangeAsync(rental1, rental4);
            await this.Context.Payments.AddRangeAsync(payment1, payment2, paymentAnother5);

            await this.Context.Contracts.AddRangeAsync(contract1, contract4);

            await this.Context.SaveChangesAsync();

            var service = new OwnerPaymentService(this.Context);

            // Act
            var rentals = await this.Context.Rentals.ToListAsync();

            var result1 = await service.GetPaymentDetailsAsync(payment1.Id, tenant1.Id);
            var result2 = await service.GetPaymentDetailsAsync(payment2.Id, home1.Owner.Id);
            var result3 = await service.GetPaymentDetailsAsync(paymentAnother5.Id, tenant4.Id);

            var expected1 = await this.Context.Rentals
                .Where(r => r.TenantId == tenant1.Id)
                .SelectMany(r => r.Payments)
                .Where(p => p.Id == payment1.Id)
                .FirstOrDefaultAsync();

            var expected2 = await this.Context.Payments
                .Where(p => p.Id == payment2.Id)
                .FirstOrDefaultAsync();

            var expected3 = await this.Context.Rentals
                .Where(r => r.TenantId == tenant4.Id)
                .SelectMany(r => r.Payments)
                .Where(p => p.Id == paymentAnother5.Id)
                .FirstOrDefaultAsync();

            // Assert
            result1.Should().BeOfType<UserPaymentDetailsServiceModel>();
            result1.To.Should().Equals(home1.Owner.FirstName + home1.Owner.LastName);
            result1.Address.Should().Equals(expected1.Rental.Home.Address);

            result2.To.Should().Equals(home2.Manager.FirstName + home2.Manager.LastName);
            result2.Address.Should().Equals(expected2.Home.Address);

            result3.To.Should().Equals(home4.Owner.FirstName + home4.Owner.LastName);
            result3.Address.Should().Equals(expected3.Rental.Home.Address);
        }
    }
}
