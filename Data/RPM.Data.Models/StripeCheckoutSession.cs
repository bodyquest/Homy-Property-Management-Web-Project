namespace RPM.Data.Models
{
    public class StripeCheckoutSession
    {
        public string Id { get; set; }

        public string PaymentId { get; set; }

        public string ToStripeAccountId { get; set; }
    }
}
