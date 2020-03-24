namespace RPM.Services.Management
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using RPM.Data;
    using RPM.Data.Models;

    public class OwnerContractService : IOwnerContractService
    {
        private readonly ApplicationDbContext context;

        public OwnerContractService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<bool>CreateRentalContractAsync(byte[] fileContent, User user, Rental rental)
        {
            var contractEntity = new Contract
            {
                Title = $"{DateTime.UtcNow.Year}_{user.UserName}",
                ContractDocument = fileContent,
                RentalId = rental.Id,
            };

            this.context.Contracts.Add(contractEntity);
            var isSuccessful = await this.context.SaveChangesAsync();

            if (isSuccessful > 0)
            {
                return true;
            }

            return false;
        }
    }
}
