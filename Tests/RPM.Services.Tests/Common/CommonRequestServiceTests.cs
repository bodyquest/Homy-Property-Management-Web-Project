namespace RPM.Services.Tests.Common
{
    using System;
    using FluentAssertions;
    using RPM.Data.Models.Enums;
    using RPM.Services.Common.Implementations;
    using RPM.Services.Common.Models.Request;
    using Xunit;

    public class CommonRequestServiceTests : BaseServiceTest
    {
        #region Service Methods
        /*
         * // async Task<bool> CreateRequestAsync(RequestCreateServiceModel model)
         */
        #endregion

        [Fact] // 1. async Task<bool> CreateRequestAsync(RequestCreateServiceModel model)
        public async void CreateRequestAsync_WithGivenRequestModel_ShouldReturnTrueIfCreated()
        {
            // Arrange
            var model = new RequestCreateServiceModel
            {
                Date = DateTime.UtcNow,
                Type = RequestType.ToRent,
                UserId = Guid.NewGuid().ToString(),
                HomeId = Guid.NewGuid().ToString(),
                Message = Guid.NewGuid().ToString(),
                Document = new byte[1024],
            };

            var service = new RequestService(this.Context);

            // Act
            var result = await service.CreateRequestAsync(model);

            // Assert
            result.Should().BeTrue();
        }
    }
}
