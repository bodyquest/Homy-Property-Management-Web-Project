namespace RPM.Services.Management.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class OwnerAllTransactionRequestsServiceModel
    {
        public string Id { get; set; }

        public string Date { get; set; }

        public string TenantName { get; set; }

        public string OwnerName { get; set; }

        public string Reason { get; set; }

        public decimal Amount { get; set; }

        public bool IsRecurring { get; set; }

        public string Status { get; set; }
    }
}
