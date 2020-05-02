namespace RPM.Services.Tests.Common
{
    using System;
    using System.Linq;
    using FluentAssertions;
    using Microsoft.EntityFrameworkCore;
    using RPM.Data.Models;
    using RPM.Services.Common.Implementations;
    using Xunit;

    public class ImageDbServiceTests : BaseServiceTest
    {
        [Fact] // 1. async Task<int> WriteToDatabasebAsync(string imageUrl, string imagePublicId)
        public async void WriteToDatabasebAsync_ShouldReturn_ImageId()
        {
            // Arrange
            var imageUrl = Guid.NewGuid().ToString();
            var imagePublicId = Guid.NewGuid().ToString();

            var service = new ImageDbService(this.Context);

            // Act
            var result = await service.WriteToDatabasebAsync(imageUrl, imagePublicId);
            var expected = await this.Context.CloudImages
                .Where(i => i.PicturePublicId == imagePublicId)
                .Select(i => i.Id)
                .FirstOrDefaultAsync();

            // Assert
            result.Should().Equals(expected);
        }

        [Fact] // 2. string GetPublicId(int id)
        public async void GetPublicId_shouldReturnImagePublicId()
        {
            // Arrange
            var imageUrl = Guid.NewGuid().ToString();
            var imagePublicId = Guid.NewGuid().ToString();
            var image = new CloudImage
            {
                PictureUrl = imageUrl,
                PicturePublicId = imagePublicId,
            };

            await this.Context.CloudImages.AddAsync(image);

            await this.Context.SaveChangesAsync();

            var service = new ImageDbService(this.Context);

            // Act
            var result = service.GetPublicId(image.Id);
            var expected = await this.Context.CloudImages
                .Where(i => i.PicturePublicId == imagePublicId)
                .Select(i => i.Id)
                .FirstOrDefaultAsync();

            // Assert
            result.Should().Equals(expected);
        }
    }
}
