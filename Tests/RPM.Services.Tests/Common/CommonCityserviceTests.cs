namespace RPM.Services.Tests.Common
{
    using FluentAssertions;
    using RPM.Services.Common.Implementations;
    using RPM.Services.Common.Models.City;
    using RPM.Services.Tests.Seed;
    using Xunit;

    public class CommonCityserviceTests : BaseServiceTest
    {
        #region Service Methods
        /*
         * // async Task<IEnumerable<CityListServiceModel>> AllCitiesAsync()
         *
         * // async Task<IEnumerable<CityListServiceModel>> AllCitiesByCountryAsync(int id)
         */
        #endregion

        [Fact] // 1. async Task<IEnumerable<CityListServiceModel>> AllCitiesAsync()
        public async void AllCitiesAsync_ShouldReturnModelCollectionOfAllCitiesInDatabase()
        {
            // Arrange
            var country = CountryCreator.Create();
            var city = CityCreator.Create(country.Id);
            var city2 = CityCreator.Create(country.Id);
            var city3 = CityCreator.Create(country.Id);

            await this.Context.Countries.AddAsync(country);
            await this.Context.Cities.AddRangeAsync(city, city2, city3);

            await this.Context.SaveChangesAsync();

            var service = new CityService(this.Context);

            // Act
            var result = await service.AllCitiesAsync();

            // Assert
            result.Should().AllBeOfType<CityListServiceModel>();
            result.Should().HaveCount(3);
        }

        [Fact] // 2. async Task<IEnumerable<CityListServiceModel>> AllCitiesByCountryAsync(int id)
        public async void AllCitiesByCountryAsync_ShouldReturnModelCollectionOfAllByCountry()
        {
            // Arrange
            var country = CountryCreator.Create();
            var city = CityCreator.Create(country.Id);
            var city2 = CityCreator.Create(country.Id);
            var city3 = CityCreator.Create(country.Id);

            var country2 = CountryCreator.Create();
            var city4 = CityCreator.Create(country2.Id);
            var city5 = CityCreator.Create(country2.Id);

            await this.Context.Countries.AddRangeAsync(country, country2);
            await this.Context.Cities.AddRangeAsync(city, city2, city3, city4, city5);

            await this.Context.SaveChangesAsync();

            var service = new CityService(this.Context);

            // Act
            var result = await service.AllCitiesByCountryAsync(country2.Id);

            // Assert
            result.Should().AllBeOfType<CityListServiceModel>();
            result.Should().HaveCount(2);
        }
    }
}
