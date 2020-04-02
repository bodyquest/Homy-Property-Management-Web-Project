namespace RPM.Services.Management
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using RPM.Data.Models;

    public interface IOwnerContractService
    {
        Task<bool> CreateRentalContractAsync(byte[] fileContent, User user, Rental rental);

        Task<bool> CreateManageContractAsync(byte[] fileContent, User user);
    }
}
