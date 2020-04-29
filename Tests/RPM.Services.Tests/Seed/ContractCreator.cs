namespace RPM.Services.Tests.Seed
{
    using System;
    using RPM.Data.Models;

    public class ContractCreator
    {
        public static Contract Create()
        {
            var rnd = new Random();

            return new Contract
            {
                Id = Guid.NewGuid().ToString(),
                Title = "New Rental Contract",
                ContractDocument = new byte[1024],
            };
        }

        public static Contract CreateRentalContract(int id)
        {
            var rnd = new Random();

            return new Contract
            {
                Id = Guid.NewGuid().ToString(),
                Title = "New Rental Contract",
                ContractDocument = new byte[1024],
                RentalId = id,
            };
        }

        public static Contract CreateManageContract(string userId)
        {
            var rnd = new Random();

            return new Contract
            {
                Id = Guid.NewGuid().ToString(),
                Title = "New Rental Contract",
                ContractDocument = new byte[1024],
                ManagerId = userId,
            };
        }
    }
}
