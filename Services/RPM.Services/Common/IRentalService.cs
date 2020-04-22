namespace RPM.Services.Common
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using RPM.Services.Common.Models.Rental;

    public interface IRentalService
    {
        Task<IEnumerable<UserRentalListServiceModel>> GetUserRentalsListAsync(string userId);

        Task<RentalInfoServiceModel> GetDetailsAsync(int id);
    }
}
