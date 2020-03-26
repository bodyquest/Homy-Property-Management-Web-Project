namespace RPM.Services.Common.Models.Profile
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using RPM.Data.Models.Enums;

    public class UserPaymentListServiceModel
    {
        public string Id { get; set; }

        public DateTime Date { get; set; }

        public string To { get; set; }

        public string Reason { get; set; }

        public decimal Amount { get; set; }

        public PaymentStatus Status { get; set; }

        public string RentalAddress { get; set; }
    }
}
