namespace RPM.Services.Common.Models.Payment
{
    using System;
    using RPM.Data.Models.Enums;

    public class UserPaymentDetailsServiceModel
    {
        public string Id { get; set; }

        public DateTime Date { get; set; }

        public DateTime? TransactionDate { get; set; }

        public string To { get; set; }

        public bool RecipientHasStripeAccount { get; set; }

        public string ToStripeAccountId { get; set; }

        public string Reason { get; set; }

        public decimal? Amount { get; set; }

        public PaymentStatus Status { get; set; }

        public string Address { get; set; }
    }
}
