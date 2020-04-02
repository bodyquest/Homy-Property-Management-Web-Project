namespace RPM.Web.Models.Payment
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using RPM.Data.Models.Enums;

    public class CheckoutPaymentViewModel
    {
        public string Id { get; set; }

        public DateTime Date { get; set; }

        public string To { get; set; }

        public string ToStripeAccountId { get; set; }

        public string Reason { get; set; }

        public decimal? Amount { get; set; }

        public PaymentStatus Status { get; set; }

        public string RentalAddress { get; set; }
    }
}
