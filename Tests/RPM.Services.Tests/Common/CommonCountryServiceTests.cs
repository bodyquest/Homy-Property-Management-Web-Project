namespace RPM.Services.Tests.Common
{
    using FluentAssertions;
    using RPM.Services.Common.Implementations;
    using RPM.Services.Common.Models.Country;
    using RPM.Services.Tests.Seed;
    using Xunit;

    public class CommonCountryServiceTests : BaseServiceTest
    {
        [Fact] // async Task<IEnumerable<CountryListServiceModel>> AllCountriesAsync()
        public async void AllCountriesAsync_ShouldReturnModelColletionOfAllCountriesInDatabase()
        {
            var country = CountryCreator.Create();
            var country2 = CountryCreator.Create();
            var country3 = CountryCreator.Create();
            var country4 = CountryCreator.Create();

            await this.Context.Countries.AddRangeAsync(country, country2, country3, country4);

            await this.Context.SaveChangesAsync();

            var service = new CountryService(this.Context);

            // Act
            var result = await service.AllCountriesAsync();

            // Assert
            result.Should().AllBeOfType<CountryListServiceModel>();
            result.Should().HaveCount(4);
        }
    }
}
