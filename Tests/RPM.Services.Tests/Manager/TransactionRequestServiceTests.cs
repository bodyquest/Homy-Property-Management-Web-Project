namespace RPM.Services.Tests.Manager
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using FluentAssertions;
    using Microsoft.EntityFrameworkCore;
    using RPM.Data.Models;
    using RPM.Data.Models.Enums;
    using RPM.Services.Management.Implementations;
    using RPM.Services.Management.Models;
    using RPM.Services.Tests.Seed;
    using RPM.Web.Areas.Management.Models.TransactionRequests;
    using Xunit;

    public class TransactionRequestServiceTests : BaseServiceTest
    {
        #region Service Methods
        /*
         * // async Task<IEnumerable<OwnerAllTransactionRequestsServiceModel>>
            GetAllTransactionRequestsAsync(string userId)
         *
         * // async Task<string> CreateAsync(
            string recipientId, OwnerTransactionRequestsCreateInputServiceModel model)
         *
         * // async Task<string> CreateToAsync(string senderId, OwnerTransactionToRequestsCreateInputServiceModel model)
         *
         * // async Task<bool> UpdateAsync(TransactionRequest transactionRequest)
         *
         * // async Task<TransactionRequest> FindByIdAsync(string id)
         */
        #endregion

        [Fact] // async Task<IEnumerable<OwnerAllTransactionRequestsServiceModel>> GetAllTransactionRequestsAsync(string userId)
        public async void GetAllTransactionRequestsAsync_ForGivenOwnerId_ShouldReturnModelCollection()
        {
            // Arrange
            var id = 1;
            var country = CountryCreator.Create();
            var city = CityCreator.Create(country.Id);
            var tenant = UserCreator.Create("Shunko", "Svinski", "shunkata", "svin@prasmail.com");
            var owner = UserCreator.Create("Suzdurma", "Saturov", "satura", "satur@prasmail.com");

            var home = HomeCreator.CreateOwnerHome(owner.Id, city.Id);
            var home2 = HomeCreator.CreateManagedHome(owner.Id, city.Id);

            var contract = ContractCreator.CreateRentalContract(id);
            var rental = RentalCreator.Create(id, country, city, tenant, home, contract);

            var anotherId = 2;
            var anotherTenant = UserCreator.Create("Fileslav", "Karadjolanov", "fileto", "fileto@prasmail.com");
            var anotherOwner = UserCreator.Create("Prasemir", "Babek", "nadenicata", "nadenicata@prasmail.com");
            var anotherHome = HomeCreator.CreateOwnerHome(anotherOwner.Id, city.Id);
            var anotherContract = ContractCreator.CreateRentalContract(anotherId);
            var anotherRental = RentalCreator.Create(anotherId, country, city, anotherTenant, anotherHome, anotherContract);

            var trId = Guid.NewGuid().ToString();
            var trId2 = Guid.NewGuid().ToString();
            var trId5 = Guid.NewGuid().ToString();

            var transactionRequest = TransactionRequestCreator.CreateForRental(trId, tenant.Id, owner.Id, rental.Id);
            var transactionRequest2 = TransactionRequestCreator.CreateForManager(trId2, owner.Id, home2.ManagerId, home2.Id);
            var transactionRequest5 = TransactionRequestCreator.CreateForRental(trId5, anotherTenant.Id, anotherOwner.Id, anotherRental.Id);

            await this.Context.Countries.AddAsync(country);
            await this.Context.Cities.AddAsync(city);
            await this.Context.Users.AddRangeAsync(owner, tenant, anotherTenant);
            await this.Context.Homes.AddRangeAsync(home, home2);
            await this.Context.Contracts.AddRangeAsync(contract, anotherContract);
            await this.Context.Rentals.AddRangeAsync(rental, anotherRental);
            await this.Context
                .TransactionRequests
                .AddRangeAsync(transactionRequest, transactionRequest2, transactionRequest5);
            await this.Context.SaveChangesAsync();

            var service = new OwnerTransactionRequestService(this.Context);

            // Act
            var result = await service.GetAllTransactionRequestsAsync(owner.Id);

            // Assert
            result.Should().AllBeOfType<OwnerAllTransactionRequestsServiceModel>();
            result.Should().HaveCount(2, "because there are 2 different contracts - one home rented and another managed");
        }

        [Fact] // async Task<string> CreateAsync(string recipientId, OwnerTransactionRequestsCreateInputServiceModel model)
        public async void CreateAsync_WithGivenRecipientIdAndRequestModel_ShouldCreateTransactionRequest()
        {
            // Arrange
            var id = 1;
            var country = CountryCreator.Create();
            var city = CityCreator.Create(country.Id);
            var tenant = UserCreator.Create("Shunko", "Svinski", "shunkata", "svin@prasmail.com");
            var owner = UserCreator.Create("Suzdurma", "Saturov", "satura", "satur@prasmail.com");

            var home = HomeCreator.CreateOwnerHome(owner.Id, city.Id);

            var contract = ContractCreator.CreateRentalContract(id);
            var rental = RentalCreator.Create(id, country, city, tenant, home, contract);

            await this.Context.Countries.AddAsync(country);
            await this.Context.Cities.AddAsync(city);
            await this.Context.Users.AddRangeAsync(owner, tenant);
            await this.Context.Homes.AddAsync(home);
            await this.Context.Contracts.AddAsync(contract);
            await this.Context.Rentals.AddRangeAsync(rental);
            await this.Context.SaveChangesAsync();

            var recipientId = Guid.NewGuid().ToString();

            var model = new OwnerTransactionRequestsCreateInputServiceModel
            {
                Id = Guid.NewGuid().ToString(),
                Reason = Guid.NewGuid().ToString(),
                RecurringSchedule = RecurringScheduleType.Monthly,
                IsRecurring = true,
                RentalId = id,
            };

            var service = new OwnerTransactionRequestService(this.Context);

            // Act
            var result = await service.CreateAsync(owner.Id, model);
            var expected = await this.Context.TransactionRequests
                .Where(x => x.RecipientId == owner.Id)
                .FirstOrDefaultAsync();

            // Assert
            result.Should().BeOfType<string>();
            result.Should().Equals(expected.Id);
        }

        [Fact] // async Task<string> CreateToAsync(string senderId, OwnerTransactionToRequestsCreateInputServiceModel model)
        public async void CreateToAsync_WithGivenSenderIdAndRequestModel_ShouldCreateTransactionRequest()
        {
            // Arrange
            var country = CountryCreator.Create();
            var city = CityCreator.Create(country.Id);
            var owner = UserCreator.Create("Suzdurma", "Saturov", "satura", "satur@prasmail.com");

            var home = HomeCreator.CreateManagedHome(owner.Id, city.Id);

            await this.Context.Countries.AddAsync(country);
            await this.Context.Cities.AddAsync(city);
            await this.Context.Users.AddAsync(owner);
            await this.Context.Homes.AddAsync(home);
            await this.Context.SaveChangesAsync();

            var recipientId = Guid.NewGuid().ToString();

            var model = new OwnerTransactionToRequestsCreateInputServiceModel
            {
                Id = Guid.NewGuid().ToString(),
                Reason = Guid.NewGuid().ToString(),
                RecurringSchedule = RecurringScheduleType.Monthly,
                IsRecurring = true,
                HomeId = home.Id,
            };

            var service = new OwnerTransactionRequestService(this.Context);

            // Act
            var result = await service.CreateToAsync(owner.Id, model);
            var expected = await this.Context.TransactionRequests
                .Where(x => x.RecipientId == home.ManagerId)
                .FirstOrDefaultAsync();

            // Assert
            result.Should().BeOfType<string>();
            result.Should().Equals(expected.Id);
        }

        [Fact] // async Task<bool> UpdateAsync(TransactionRequest transactionRequest)
        public async void UpdateAsync_ForGivenTransactionRequestObject_ShouldReturnTrueIfUpdatedSuccessfully()
        {
            // Arrange
            var senderId = Guid.NewGuid().ToString();
            var recipientId = Guid.NewGuid().ToString();

            var trId = Guid.NewGuid().ToString();

            var transactionRequest = TransactionRequestCreator.Create(trId, senderId, recipientId);

            await this.Context.TransactionRequests.AddAsync(transactionRequest);
            await this.Context.SaveChangesAsync();

            var statusBefore = transactionRequest.Status;
            var statusAfter = TransactionRequestStatus.Complete;
            transactionRequest.Status = statusAfter;

            var service = new OwnerTransactionRequestService(this.Context);

            // Act
            var result = await service.UpdateAsync(transactionRequest);
            var changed = await this.Context.TransactionRequests.FindAsync(trId);

            // Assert
            result.Should().BeTrue();
            changed.Status.Should().Equals(statusAfter);
            changed.Status.Should().NotBe(statusBefore);
        }

        [Fact] // async Task<TransactionRequest> FindByIdAsync(string id)
        public async void FindByIdAsync_ForTransactionRequestId_ShouldReturnTheFoundObject()
        {
            // Arrange
            var senderId = Guid.NewGuid().ToString();
            var recipientId = Guid.NewGuid().ToString();

            var trId = Guid.NewGuid().ToString();
            var trId2 = Guid.NewGuid().ToString();
            var trId3 = Guid.NewGuid().ToString();

            var transactionRequest = TransactionRequestCreator.Create(trId, senderId, recipientId);
            var transactionRequest2 = TransactionRequestCreator.Create(trId2, senderId, recipientId);
            var transactionRequest3 = TransactionRequestCreator.Create(trId3, senderId, recipientId);

            await this.Context.TransactionRequests.AddRangeAsync(transactionRequest, transactionRequest2, transactionRequest3);
            await this.Context.SaveChangesAsync();

            var service = new OwnerTransactionRequestService(this.Context);

            // Act
            var result = await service.FindByIdAsync(trId);
            var result2 = await service.FindByIdAsync(trId2);
            var expected = trId;
            var expected2 = trId2;

            // Assert
            result.Should().BeOfType<TransactionRequest>();
            result.Id.Should().Equals(expected);
            result2.Id.Should().Equals(expected2);
        }
    }
}
