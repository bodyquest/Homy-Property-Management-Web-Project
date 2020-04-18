namespace RPM.Services.Common.Models.Dashboard
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using RPM.Data.Models.Enums;

    public class ManagerPaymentListServiceModel
    {
        public string Id { get; set; }

        public DateTime Date { get; set; }

        public DateTime? TransactionDate { get; set; }

        public string From { get; set; }

        public string Reason { get; set; }

        public decimal Amount { get; set; }

        public PaymentStatus Status { get; set; }

        public string Address { get; set; }

    }
}
