namespace RPM.Services.Tests.Manager
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using FluentAssertions;
    using Microsoft.EntityFrameworkCore;
    using RPM.Data.Models;
    using RPM.Data.Models.Enums;
    using RPM.Services.Management.Implementations;
    using RPM.Services.Management.Models;
    using RPM.Services.Tests.Seed;
    using Xunit;

    public class RequestServiceTests : BaseServiceTest
    {
        #region Service Methods
        /*
         * // async Task<IEnumerable<OwnerIndexRequestsServiceModel>> GetRequestsAsync(string id)
         *
         * // async Task<IEnumerable<OwnerAllRequestsServiceModel>> GetAllRequestsWthDetailsAsync(string id)
         *
         * // async Task<Request> ApproveRequestAsync(string id)
         *
         * // async Task<bool> RejectRequestAsync(string id)
         *
         * // async Task<byte[]> GetFileAsync(string requestId)
         *
         * // async Task<OwnerRequestInfoServiceModel> GetRequestInfoAsync(string requestId)
         *
         * // async Task<OwnerRequestDetailsServiceModel> GetRequestDetailsAsync(string requestId)
         */
        #endregion

        [Fact] // 1. async Task<IEnumerable<OwnerIndexRequestsServiceModel>> GetRequestsAsync(string id)
        public async void GetRequestsAsync_WithGivenOwnerId_ShouldReturnCollectionOfRequests()
        {
            // Arrange
            var country = CountryCreator.Create();
            var city = CityCreator.Create(country.Id);
            var owner = UserCreator.Create("Suzdurma", "Saturov", "satura", "satur@prasmail.com");
            var anotherOwner = UserCreator.Create("Svinevud", "Kulchibutov", "kulkata", "svinevuda@prasmail.com");
            var home = HomeCreator.CreateOwnerHome(owner.Id, city.Id);
            var home2 = HomeCreator.CreateOwnerHome(owner.Id, city.Id);
            var homе3 = HomeCreator.CreateOwnerHome(owner.Id, city.Id);
            var homе4 = HomeCreator.CreateOwnerHome(anotherOwner.Id, city.Id);

            var request = RequestCreator.Create(home);
            var request2 = RequestCreator.Create(home2);
            var request3 = RequestCreator.Create(homе3);
            var request4 = RequestCreator.Create(homе4);

            await this.Context.Countries.AddAsync(country);
            await this.Context.Cities.AddAsync(city);
            await this.Context.Users.AddRangeAsync(owner, anotherOwner);
            await this.Context.Homes.AddRangeAsync(home, home2, homе3, homе4);
            await this.Context.Requests.AddRangeAsync(request, request2, request3, request4);

            await this.Context.SaveChangesAsync();

            var service = new OwnerRequestService(this.Context);

            // Act
            var result = await service.GetRequestsAsync(owner.Id);
            var expected = await this.Context.Requests.Where(r => r.Home.OwnerId == owner.Id).ToListAsync();

            // Assert
            result.Should().AllBeOfType<OwnerIndexRequestsServiceModel>();
            result.Count().Should().Equals(expected.Count());
            result.Should().HaveCount(3, "because there are 3 homes for which the owner received requests individually");
        }

        [Fact] // 2. async Task<IEnumerable<OwnerAllRequestsServiceModel>> GetAllRequestsWthDetailsAsync(string id)
        public async void GetAllRequestsWthDetailsAsync_WithGivenOwnerId_ShouldReturnCollectionOfRequests()
        {
            // Arrange
            var country = CountryCreator.Create();
            var city = CityCreator.Create(country.Id);
            var owner = UserCreator.Create("Suzdurma", "Saturov", "satura", "satur@prasmail.com");
            var anotherOwner = UserCreator.Create("Svinevud", "Kulchibutov", "kulkata", "svinevuda@prasmail.com");
            var home = HomeCreator.CreateOwnerHome(owner.Id, city.Id);
            var home2 = HomeCreator.CreateOwnerHome(owner.Id, city.Id);
            var homе3 = HomeCreator.CreateOwnerHome(owner.Id, city.Id);
            var homе4 = HomeCreator.CreateOwnerHome(anotherOwner.Id, city.Id);

            var request = RequestCreator.Create(home);
            var request2 = RequestCreator.Create(home2);
            var request3 = RequestCreator.Create(homе3);
            var request4 = RequestCreator.Create(homе4);

            request2.User.UserName = "fileto";
            request3.User.UserName = "butcheto";

            await this.Context.Countries.AddAsync(country);
            await this.Context.Cities.AddAsync(city);
            await this.Context.Users.AddRangeAsync(owner, anotherOwner);
            await this.Context.Homes.AddRangeAsync(home, home2, homе3, homе4);
            await this.Context.Requests.AddRangeAsync(request, request2, request3, request4);

            await this.Context.SaveChangesAsync();

            var service = new OwnerRequestService(this.Context);

            // Act
            var result = (await service.GetAllRequestsWthDetailsAsync(owner.Id)).ToList();
            var expected = await this.Context.Requests.Where(r => r.Home.OwnerId == owner.Id).ToListAsync();

            // Assert
            result.Should().AllBeOfType<OwnerAllRequestsServiceModel>();
            result.Count().Should().Equals(expected.Count());
            result.Should().HaveCount(3, "because there are 3 homes for which the owner received requests individually");
            result[0].Username = request.User.UserName; // shpeka
            result[1].Username = request2.User.UserName; // fileto
            result[2].Username = request3.User.UserName; // butcheto
        }

        [Fact] // 3. async Task<Request> ApproveRequestAsync(string id)
        public async void ApproveRequestAsync_WithGivenId_ShouldApproveRequest_AndReturnTheRequest()
        {
            // Arrange
            var country = CountryCreator.Create();
            var city = CityCreator.Create(country.Id);
            var owner = UserCreator.Create("Suzdurma", "Saturov", "satura", "satur@prasmail.com");
            var home = HomeCreator.CreateOwnerHome(owner.Id, city.Id);
            var home2 = HomeCreator.CreateOwnerHome(owner.Id, city.Id);

            var request = RequestCreator.Create(home);
            var request2 = RequestCreator.Create(home2);

            var requestStatusBefore = request.Status;
            var request2StatusBefore = request2.Status;

            await this.Context.Countries.AddAsync(country);
            await this.Context.Cities.AddAsync(city);
            await this.Context.Users.AddAsync(owner);
            await this.Context.Homes.AddRangeAsync(home, home2);
            await this.Context.Requests.AddRangeAsync(request, request2);

            await this.Context.SaveChangesAsync();

            var service = new OwnerRequestService(this.Context);

            // Act
            var result = await service.ApproveRequestAsync(request.Id);
            var result2 = await service.ApproveRequestAsync(request2.Id);
            var expected = RequestStatus.Approved;
            var expected2 = RequestStatus.Approved;

            // Assert
            result.Should().BeOfType<Request>();
            result.Status.Should().Equals(expected);
            result2.Status.Should().Equals(expected2);
            result.Status.Should().NotBe(requestStatusBefore);
            result2.Status.Should().NotBe(request2StatusBefore);
        }

        [Fact] // 4. async Task<bool> RejectRequestAsync(string id)
        public async void RejectRequestAsync_WithGivenId_ShouldRejectRequest_AndReturnTrue()
        {
            // Arrange
            var country = CountryCreator.Create();
            var city = CityCreator.Create(country.Id);
            var owner = UserCreator.Create("Suzdurma", "Saturov", "satura", "satur@prasmail.com");
            var anotherOwner = UserCreator.Create("Svinevud", "Kulchibutov", "kulkata", "svinevuda@prasmail.com");
            var home = HomeCreator.CreateOwnerHome(owner.Id, city.Id);
            var home2 = HomeCreator.CreateOwnerHome(owner.Id, city.Id);

            var request = RequestCreator.Create(home);
            var request2 = RequestCreator.Create(home2);

            var requestStatusBefore = request.Status;
            var request2StatusBefore = request2.Status;

            await this.Context.Countries.AddAsync(country);
            await this.Context.Cities.AddAsync(city);
            await this.Context.Users.AddAsync(owner);
            await this.Context.Homes.AddRangeAsync(home, home2);
            await this.Context.Requests.AddRangeAsync(request, request2);

            await this.Context.SaveChangesAsync();

            var service = new OwnerRequestService(this.Context);

            // Act
            var result = await service.RejectRequestAsync(request.Id);
            var result2 = await service.RejectRequestAsync(request2.Id);

            // Assert
            result.Should().BeTrue();
            result2.Should().BeTrue();
        }

        [Fact] // 5. async Task<byte[]> GetFileAsync(string requestId)
        public async void GetFileAsync_WithGivenRequestId_ShouldReturnByteArrayOfTheFile()
        {
            // Arrange
            var country = CountryCreator.Create();
            var city = CityCreator.Create(country.Id);
            var owner = UserCreator.Create("Suzdurma", "Saturov", "satura", "satur@prasmail.com");
            var anotherOwner = UserCreator.Create("Svinevud", "Kulchibutov", "kulkata", "svinevuda@prasmail.com");
            var home = HomeCreator.CreateOwnerHome(owner.Id, city.Id);

            var request = RequestCreator.Create(home);
            request.Document = new byte [1024];

            await this.Context.Countries.AddAsync(country);
            await this.Context.Cities.AddAsync(city);
            await this.Context.Users.AddAsync(owner);
            await this.Context.Homes.AddAsync(home);
            await this.Context.Requests.AddAsync(request);

            await this.Context.SaveChangesAsync();

            var service = new OwnerRequestService(this.Context);

            // Act
            var result = await service.GetFileAsync(request.Id);
            var expected = await this.Context.Requests
                .Where(r => r.Id == request.Id)
                .Select(r => r.Document)
                .FirstOrDefaultAsync();

            // Assert
            result.Should().BeOfType<byte[]>();
            result.Should().Equals(expected);
        }

        [Fact] // 6. async Task<OwnerRequestInfoServiceModel> GetRequestInfoAsync(string requestId)
        public async void GetRequestInfoAsync_WithGivenRequestId_ShouldReturnServiceModel()
        {
            // Arrange
            var country = CountryCreator.Create();
            var city = CityCreator.Create(country.Id);
            var owner = UserCreator.Create("Suzdurma", "Saturov", "satura", "satur@prasmail.com");
            var home = HomeCreator.CreateOwnerHome(owner.Id, city.Id);

            var request = RequestCreator.Create(home);

            var doc = new byte[1024];
            request.Document = doc;

            await this.Context.Countries.AddAsync(country);
            await this.Context.Cities.AddAsync(city);
            await this.Context.Users.AddAsync(owner);
            await this.Context.Homes.AddAsync(home);
            await this.Context.Requests.AddAsync(request);

            await this.Context.SaveChangesAsync();

            var model = new OwnerRequestInfoServiceModel
            {
                UserId = request.User.Id,
                UserFirstName = request.User.FirstName,
                UserLastName = request.User.LastName,
                RequestType = request.Type.ToString(),
                Document = doc,
            };

            var service = new OwnerRequestService(this.Context);

            // Act
            var result = await service.GetRequestInfoAsync(request.Id);
            var expected = model;

            // Assert
            result.Should().BeOfType<OwnerRequestInfoServiceModel>();
            result.Should().Equals(expected);
        }

        [Fact] // 7. async Task<OwnerRequestDetailsServiceModel> GetRequestDetailsAsync(string requestId)
        public async void GetRequestDetailsAsync_WithGivenRequestId_ShouldReturnServiceModel()
        {
            // Arrange
            var country = CountryCreator.Create();
            var city = CityCreator.Create(country.Id);
            var owner = UserCreator.Create("Suzdurma", "Saturov", "satura", "satur@prasmail.com");
            var home = HomeCreator.CreateOwnerHome(owner.Id, city.Id);

            var request = RequestCreator.Create(home);

            var doc = new byte[1024];
            request.Document = doc;

            await this.Context.Countries.AddAsync(country);
            await this.Context.Cities.AddAsync(city);
            await this.Context.Users.AddAsync(owner);
            await this.Context.Homes.AddAsync(home);
            await this.Context.Requests.AddAsync(request);

            await this.Context.SaveChangesAsync();

            var model = new OwnerRequestDetailsServiceModel
            {
                Id = request.Id,
                Date = request.Date.ToString("dd/MM/yyyy h:mm tt"),
                UserFirstName = request.User.FirstName,
                UserLastName = request.User.LastName,
                Email = request.User.Email,
                Phone = request.User.PhoneNumber,
                RequestType = request.Type.ToString(),
                Document = doc,
            };

            var service = new OwnerRequestService(this.Context);

            // Act
            var result = await service.GetRequestDetailsAsync(request.Id);
            var expected = model;

            // Assert
            result.Should().BeOfType<OwnerRequestDetailsServiceModel>();
            result.Should().Equals(expected);
        }
    }
}
