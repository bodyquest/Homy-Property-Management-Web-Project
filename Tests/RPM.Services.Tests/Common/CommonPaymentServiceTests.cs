namespace RPM.Services.Tests.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using RPM.Data.Models;
    using RPM.Data.Models.Enums;
    using RPM.Services.Common.Implementations;
    using RPM.Services.Common.Models.Dashboard;
    using RPM.Services.Management;
    using RPM.Services.Tests.Seed;
    using Stripe;
    using Stripe.Checkout;
    using Xunit;

    using static RPM.Common.GlobalConstants;

    public class CommonPaymentServiceTests : BaseServiceTest
    {
        #region Service Methods
        /*
         * // 1. async Task<IEnumerable<UserPaymentListServiceModel>> GetUserPaymentsListAsync(string userId)
         *
         * // 2. async Task<IEnumerable<ManagerPaymentListServiceModel>> GetManagerPaymentsListAsync(string userId)
         *
         * // 3. async Task<UserPaymentDetailsServiceModel> GetPaymentDetailsAsync(string paymentId, string userId)
         *
         * // 4. async Task<bool> EditPaymentStatusAsync(string paymentId, string userId, PaymentStatus status, DateTime? date)
         *
         * // 5. async Task<bool> AddPaymentRequestToUserAsync(string userId, string requestId)
         *
         * // 6. async Task CreateCheckoutSessionAsync(string sessionId, string paymentId, string toStripeAccountId)
         *
         * // 7. async Task<bool> MarkPaymentAsCompletedAsync(Session session)
         *
         * // 8. async Task<bool> CompareData(string sessionId)
         *
         * // 9. async Task<string> GetPaymentId(string sessionId)
         */
        #endregion

        [Fact] // 1. async Task<IEnumerable<UserPaymentListServiceModel>> GetUserPaymentsListAsync(string userId)
        public async void GetUserPaymentsListAsync_ForGivenUserId_ShouldReturnAllRelatedPayments()
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

            var service = new PaymentCommonService(this.Context, null, null, null);

            //// Act
            //var result = (await service.AllPayments(home1.Owner.Id)).ToList();
            //var expectedCount = this.Context.Payments
            //    .Where(p => p.Home.OwnerId == home1.Owner.Id || p.Rental.Home.OwnerId == home1.Owner.Id)
            //    .Count();

            //// Assert
            //result.Should().AllBeOfType<OwnerAllPaymentsServiceModel>();
            //result.Should().HaveCount(expectedCount);
            //result.Should().HaveCount(4, "because there are 3 properties where one has tenant + manager, which makes 4");
        }

        [Fact] // 2. async Task<IEnumerable<ManagerPaymentListServiceModel>> GetManagerPaymentsListAsync(string userId)
        public async void GetManagerPaymentsListAsync_ForGivenManagerId_ShouldReturnCollectionModel()
        {
            // Arrange
            var country = CountryCreator.Create();
            var city = CityCreator.Create(country.Id);
            var manager = UserCreator.Create("Georgi", "Butov", "joro", "joro@prasmail.com");

            var home1 = HomeCreator.CreateAny(city.Id); // rented
            var home2 = HomeCreator.CreateAny(city.Id); // managed
            var home3 = HomeCreator.CreateAny(city.Id); // managed
            home2.Manager = manager;
            home2.OwnerId = home2.Owner.Id;
            home2.ManagerId = manager.Id;
            home3.Manager = manager;
            home3.OwnerId = home3.Owner.Id;
            home3.ManagerId = manager.Id;

            var payment2 = PaymentCreator.CreateForManager(home2.Owner.Id, home2.Manager.Id, home2.Id);
            var payment3 = PaymentCreator.CreateForManager(home3.Owner.Id, home2.Manager.Id, home3.Id);

            await this.Context.Users.AddAsync(manager);
            await this.Context.Countries.AddAsync(country);
            await this.Context.Cities.AddAsync(city);
            await this.Context.Homes.AddRangeAsync(home1, home2, home3);
            await this.Context.Payments.AddRangeAsync(payment2, payment3);

            await this.Context.SaveChangesAsync();

            var service = new PaymentCommonService(this.Context, null, null, null);

            // Act
            var result = await service.GetManagerPaymentsListAsync(home2.Manager.Id);

            var expected = await this.Context.Homes
                .Where(h => h.ManagerId == home2.Manager.Id)
                .SelectMany(h => h.Payments)
                .ToListAsync();

            // Assert
            result.Should().AllBeOfType<ManagerPaymentListServiceModel>();
            result.Count().Should().Equals(2);
            result.Count().Should().Equals(expected.Count());
        }

        [Fact] // 3. async Task<UserPaymentDetailsServiceModel> GetPaymentDetailsAsync(string paymentId, string userId)
        public async void GetPaymentDetailsAsync_WithGivenPaymentIdAndUserId_ShouldReturnDetailsModel()
        {
            // Arrange
            var country = CountryCreator.Create();
            var city = CityCreator.Create(country.Id);

            var home1 = HomeCreator.CreateAny(city.Id); // rented
            var home2 = HomeCreator.CreateManagedHome(home1.Owner.Id, city.Id); // managed

            var tenant1 = UserCreator.Create("Debelin", "Dignibutov", "but4eto", "but@prasmail.com");

            int id1 = 1;
            var contract1 = ContractCreator.CreateRentalContract(id1);
            var rental1 = RentalCreator.Create(id1, country, city, tenant1, home1, contract1);

            var payment1 = PaymentCreator.CreateForTenant(home1.Owner, tenant1.Id, rental1.Id);
            var payment2 = PaymentCreator.CreateForManager(home1.Owner.Id, home2.Manager.Id, home2.Id);

            await this.Context.Countries.AddAsync(country);
            await this.Context.Cities.AddAsync(city);
            await this.Context.Homes.AddRangeAsync(home1, home2);
            await this.Context.Users.AddAsync(tenant1);
            await this.Context.Rentals.AddAsync(rental1);
            await this.Context.Payments.AddRangeAsync(payment1, payment2);
            await this.Context.Contracts.AddRangeAsync(contract1);

            await this.Context.SaveChangesAsync();

            var testService = new PaymentCommonService(this.Context, null, null, null);

            // Act

            // Assert

        }

        [Fact] // 4. async Task<bool> EditPaymentStatusAsync(string paymentId, string userId, PaymentStatus status, DateTime? date)
        public async void EditPaymentStatusAsync_WithGivenPaymentId_UserId_PaymentStatusEnum_Date__ShouldReturnTrueIfEdited()
        {
            // Arrange
            var country = CountryCreator.Create();
            var city = CityCreator.Create(country.Id);

            var home1 = HomeCreator.CreateAny(city.Id); // rented
            var home2 = HomeCreator.CreateManagedHome(home1.Owner.Id, city.Id); // managed

            var tenant1 = UserCreator.Create("Debelin", "Dignibutov", "but4eto", "but@prasmail.com");

            int id1 = 1;
            var contract1 = ContractCreator.CreateRentalContract(id1);
            var rental1 = RentalCreator.Create(id1, country, city, tenant1, home1, contract1);

            var payment1 = PaymentCreator.CreateForTenant(home1.Owner, tenant1.Id, rental1.Id);
            var payment2 = PaymentCreator.CreateForManager(home1.Owner.Id, home2.Manager.Id, home2.Id);

            rental1.Payments.Add(payment1);

            await this.Context.Countries.AddAsync(country);
            await this.Context.Cities.AddAsync(city);
            await this.Context.Homes.AddRangeAsync(home1, home2);
            await this.Context.Users.AddAsync(tenant1);
            await this.Context.Rentals.AddAsync(rental1);
            await this.Context.Payments.AddRangeAsync(payment1, payment2);
            await this.Context.Contracts.AddRangeAsync(contract1);

            await this.Context.SaveChangesAsync();

            var testService = new PaymentCommonService(this.Context, null, null, null);

            // Act
            var date = DateTime.UtcNow;
            var result = await testService.EditPaymentStatusAsync(payment1.Id, tenant1.Id, PaymentStatus.Complete, date);

            // Assert
            result.Should().BeTrue();
        }

        [Fact] // 5. async Task<bool> AddPaymentRequestToUserAsync(string userId, string requestId)
        public async void AddPaymentRequestToUserAsync_WithGivenUserIdAndRequestId_ShouldAddPaymentToUserSuccessfully()
        {
            // Arrange
            var ownerId = Guid.NewGuid().ToString();
            var anotherOwnerId = Guid.NewGuid().ToString();
            var country = CountryCreator.Create();
            var city = CityCreator.Create(country.Id);

            var home1 = HomeCreator.CreateAny(city.Id); // rented
            var home2 = HomeCreator.CreateManagedHome(ownerId, city.Id); // managed

            var tenant1 = UserCreator.Create("Debelin", "Butov", "but4eto", "but@prasmail.com");

            int id1 = 1;
            var contract1 = ContractCreator.CreateRentalContract(id1);
            var rental1 = RentalCreator.Create(id1, country, city, tenant1, home1, contract1);

            var payment1 = PaymentCreator.CreateForTenant(home1.Owner, tenant1.Id, rental1.Id);
            var payment2 = PaymentCreator.CreateForManager(home1.Owner.Id, home2.Manager.Id, home2.Id);

            var senderId = Guid.NewGuid().ToString();
            var recipientId = Guid.NewGuid().ToString();

            var trId = Guid.NewGuid().ToString();

            var transactionRequest = TransactionRequestCreator
                .CreateForManager(trId, home1.Owner.Id, home2.Manager.Id, home2.Id);

            await this.Context.Countries.AddAsync(country);
            await this.Context.Cities.AddAsync(city);
            await this.Context.Homes.AddAsync(home1);
            await this.Context.Users.AddAsync(tenant1);
            await this.Context.Rentals.AddRangeAsync(rental1);
            await this.Context.Payments.AddRangeAsync(payment1, payment2);
            await this.Context.Contracts.AddRangeAsync(contract1);
            await this.Context.TransactionRequests.AddRangeAsync(transactionRequest);

            await this.Context.SaveChangesAsync();

            var transactionRequestService = new Mock<IOwnerTransactionRequestService>();
            transactionRequestService.Setup(x => x.FindByIdAsync(trId))
                .Returns(Task.FromResult(transactionRequest));

            var service = new PaymentCommonService(
                this.Context,
                transactionRequestService.Object,
                this.UserManager.Object,
                null);

            // Act
            var result = await service.AddPaymentRequestToUserAsync(home1.Owner.Id, trId);
            var createdPaymentTo = await this.Context.Payments
                .Where(p => p.HomeId == home2.Id)
                .FirstOrDefaultAsync();

            // Assert
            result.Should().BeTrue();
            createdPaymentTo.Should().NotBeNull();
        }

        [Fact] // 7. async Task<bool> MarkPaymentAsCompletedAsync(Session session)
        public async void MarkPaymentAsCompletedAsync_WithGivenSession_()
        {
            // Arrange
            var country = CountryCreator.Create();
            var city = CityCreator.Create(country.Id);

            var home1 = HomeCreator.CreateAny(city.Id); // rented
            var home2 = HomeCreator.CreateManagedHome(home1.Owner.Id, city.Id); // managed

            var tenant1 = UserCreator.Create("Debelin", "Dignibutov", "but4eto", "but@prasmail.com");

            int id1 = 1;
            var contract1 = ContractCreator.CreateRentalContract(id1);
            var rental1 = RentalCreator.Create(id1, country, city, tenant1, home1, contract1);

            var payment1 = PaymentCreator.CreateForTenant(home1.Owner, tenant1.Id, rental1.Id);
            var payment2 = PaymentCreator.CreateForManager(home1.Owner.Id, home2.Manager.Id, home2.Id);

            var id = Guid.NewGuid().ToString();
            var toStripeAccountId = "acct_1GUy2RB3QW0Kx8nS";

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string>
                {
                    "card",
                },

                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        Quantity = 1,
                        Amount = (long)payment1.Amount * 100,
                        Currency = CurrencyUSD,

                        Name = $"Rent Payment for {DateTime.UtcNow.ToString("MMMM")}/ {DateTime.UtcNow.Year}",
                    },
                },

                PaymentIntentData = new SessionPaymentIntentDataOptions
                {
                    ApplicationFeeAmount = (long)((payment1.Amount * 0.01m) * 100),
                    CaptureMethod = "automatic",
                    Description = payment1.Id,

                    TransferData = new SessionPaymentIntentTransferDataOptions
                    {
                        Destination = toStripeAccountId,
                    },
                },

                SuccessUrl = "https://homy.azurewebsites.net/checkout/success?sessionId={CHECKOUT_SESSION_ID}",
                CancelUrl = "https://homy.azurewebsites.net/checkout/cancel",
            };

            var service = new SessionService();
            Session session = service.Create(options, new RequestOptions
            {
                ApiKey = HomyTestSecretKey,
            });

            var sessionForDb = new StripeCheckoutSession
            {
                Id = session.Id,
                PaymentId = payment1.Id,
                ToStripeAccountId = toStripeAccountId,
            };

            await this.Context.Countries.AddAsync(country);
            await this.Context.Cities.AddAsync(city);
            await this.Context.Homes.AddRangeAsync(home1, home2);
            await this.Context.Users.AddAsync(tenant1);
            await this.Context.Rentals.AddAsync(rental1);
            await this.Context.Payments.AddRangeAsync(payment1, payment2);
            await this.Context.Contracts.AddRangeAsync(contract1);
            await this.Context.StripeCheckoutSessions.AddAsync(sessionForDb);

            await this.Context.SaveChangesAsync();

            var testService = new PaymentCommonService(this.Context, null, null, null);

            // Act
            var result = await testService.MarkPaymentAsCompletedAsync(session);
            bool expected = await this.Context.Payments.Where(p => p.Id == payment1.Id)
                .AnyAsync(p => p.Status == PaymentStatus.Complete);

            // Assert
            result.Should().Equals(expected);
            result.Should().BeTrue();
        }

        [Fact] // 8. async Task<bool> CompareData(string sessionId)
        public async void CompareData_WithGivenSessionId_ShouldReturnTrueIfSessionExists()
        {
            // Arrange
            var id = Guid.NewGuid().ToString();
            var paymentId = Guid.NewGuid().ToString();
            var toStripeAccountId = Guid.NewGuid().ToString();

            var session = new StripeCheckoutSession
            {
                Id = id,
                PaymentId = paymentId,
                ToStripeAccountId = toStripeAccountId,
            };

            await this.Context.StripeCheckoutSessions.AddAsync(session);
            await this.Context.SaveChangesAsync();

            var falseId = Guid.NewGuid().ToString();
            var service = new PaymentCommonService(this.Context, null, null, null);

            // Act
            var result = await service.CompareData(session.Id);
            var result2 = await service.CompareData(falseId);
            var expected = await this.Context.StripeCheckoutSessions
                .Where(s => s.Id == session.Id)
                .Select(s => s.PaymentId)
                .FirstOrDefaultAsync();

            // Assert
            result.Should().BeTrue();
            result.Should().Equals(expected);
            result2.Should().BeFalse();
        }

        [Fact] // 9. async Task<string> GetPaymentId(string sessionId)
        public async void GetPaymentId_WithGivenSessionId_ShouldReturnPaymentId()
        {
            // Arrange
            var id = Guid.NewGuid().ToString();
            var paymentId = Guid.NewGuid().ToString();
            var toStripeAccountId = Guid.NewGuid().ToString();

            var session = new StripeCheckoutSession
            {
                Id = id,
                PaymentId = paymentId,
                ToStripeAccountId = toStripeAccountId,
            };

            await this.Context.StripeCheckoutSessions.AddAsync(session);
            await this.Context.SaveChangesAsync();

            var service = new PaymentCommonService(this.Context, null, null, null);
            // Act
            var result = await service.GetPaymentId(session.Id);
            var expected = await this.Context.StripeCheckoutSessions
                .Where(s => s.Id == session.Id)
                .Select(s => s.PaymentId)
                .FirstOrDefaultAsync();

            // Assert
            result.Should().Equals(expected);
        }
    }
}
