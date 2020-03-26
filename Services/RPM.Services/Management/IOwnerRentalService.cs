namespace RPM.Services.Management
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using RPM.Services.Management.Models;

    public interface IOwnerRentalService
    {
        Task<bool> StartRent(string id, byte[] fileContent);

        Task<IEnumerable<OwnerAllRentalsServiceModel>> GetAllRentalsWithDetailsAsync(string id);

        Task<IEnumerable<OwnerIndexRentalServiceModel>> GetRentalsAsync(string userId);

        Task<IEnumerable<OwnerTransactionListOfRentalsServiceModel>>
            GetTransactionRentalsAsync(string userId);
    }
}
