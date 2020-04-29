namespace RPM.Services.Management
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using RPM.Data.Models;
    using RPM.Services.Management.Models;

    public interface IOwnerRentalService
    {
        Task<bool> StartRent(string id, byte[] fileContent);

        Task<Rental> CreateRental(string homeId, string tenantId);

        Task<bool> StopRentAsync(string id);

        Task<IEnumerable<OwnerAllRentalsServiceModel>> GetAllRentalsWithDetailsAsync(string id);

        Task<IEnumerable<OwnerIndexRentalServiceModel>> GetRentalsAsync(string userId);

        Task<IEnumerable<OwnerTransactionListOfRentalsServiceModel>>
            GetTransactionRentalsAsync(string userId);
    }
}
