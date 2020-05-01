namespace RPM.Services.Tests.Seed
{
    using System;
    using RPM.Data.Models;
    using RPM.Data.Models.Enums;

    public class TransactionRequestCreator
    {
        public static TransactionRequest Create(string id, string sender, string recipient)
        {
            var number = new Random();

            return new TransactionRequest
            {
                Id = id,
                Date = DateTime.UtcNow,
                PaymentDate = DateTime.UtcNow,
                RecipientId = recipient,
                SenderId = sender,
                Reason = "A reasonable reason",
                Amount = number.Next(50, 2000),
                RecurringSchedule = RecurringScheduleType.Monthly,
                Status = TransactionRequestStatus.Scheduled,
                RentalId = number.Next(1, 2000),
                HomeId = Guid.NewGuid().ToString(),
                IsRecurring = true,
            };
        }

        public static TransactionRequest CreateForRental(string id, string sender, string recipient, int rentalId)
        {
            var number = new Random();

            return new TransactionRequest
            {
                Id = id,
                Date = DateTime.UtcNow,
                PaymentDate = DateTime.UtcNow,
                RecipientId = recipient,
                SenderId = sender,
                Reason = "A reasonable reason",
                Amount = number.Next(50, 2000),
                RecurringSchedule = RecurringScheduleType.Monthly,
                Status = TransactionRequestStatus.Scheduled,
                RentalId = rentalId,
                IsRecurring = true,
            };
        }

        public static TransactionRequest CreateForManager(string id, string sender, string recipient, string homeId)
        {
            var number = new Random();

            return new TransactionRequest
            {
                Id = id,
                Date = DateTime.UtcNow,
                PaymentDate = DateTime.UtcNow,
                RecipientId = recipient,
                SenderId = sender,
                Reason = "A reasonable reason",
                Amount = number.Next(50, 2000),
                RecurringSchedule = RecurringScheduleType.Monthly,
                Status = TransactionRequestStatus.Scheduled,
                RentalId = null,
                HomeId = homeId,
                IsRecurring = true,
            };
        }

        public static TransactionRequest CreateForManagerWithTenant(string id, string sender, string recipient, int rentalId, string homeId)
        {
            var number = new Random();

            return new TransactionRequest
            {
                Id = id,
                Date = DateTime.UtcNow,
                PaymentDate = DateTime.UtcNow,
                RecipientId = recipient,
                SenderId = sender,
                Reason = "A reasonable reason",
                Amount = number.Next(50, 2000),
                RecurringSchedule = RecurringScheduleType.Monthly,
                Status = TransactionRequestStatus.Scheduled,
                RentalId = rentalId,
                HomeId = homeId,
                IsRecurring = true,
            };
        }
    }
}
