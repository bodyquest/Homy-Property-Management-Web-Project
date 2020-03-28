namespace RPM.Common.PaymentGateways.Stripe
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class StripeSettings
    {
        public string SecretKey { get; set; }

        public string PublishableKey { get; set; }
    }
}
