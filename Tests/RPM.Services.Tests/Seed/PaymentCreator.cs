namespace RPM.Services.Tests.Seed
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using RPM.Data.Models;
    using RPM.Data.Models.Enums;

    public class PaymentCreator
    {
        public static Payment CreateForTenant(User owner, string tenantId, int rentalId)
        {
            var rnd = new Random();

            return new Payment
            {
                Id = Guid.NewGuid().ToString(),
                Date = DateTime.UtcNow,
                Reason = "Monthly Rent",
                SenderId = tenantId,
                Recipient = owner,
                RecipientId = owner.Id,
                Amount = rnd.Next(50, 10000),
                Status = PaymentStatus.Waiting,
                RentalId = rentalId,
            };
        }

        public static Payment CreateForManager(string ownerId, string managerId, string homeId)
        {
            var rnd = new Random();

            return new Payment
            {
                Id = Guid.NewGuid().ToString(),
                Date = DateTime.UtcNow,
                Reason = "Monthly Manage Fee",
                SenderId = ownerId,
                RecipientId = managerId,
                Amount = rnd.Next(50, 10000),
                Status = PaymentStatus.Waiting,
                HomeId = homeId,
            };
        }
    }
}
