namespace RPM.Services.Management.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Text;
    using RPM.Data.Models;
    using RPM.Data.Models.Enums;

    public class OwnerAllPaymentsServiceModel
    {
        public string Id { get; set; }

        public string Date { get; set; }

        public string TransactionDate { get; set; }

        public string HomeOwnerName { get; set; }

        public string RentalHomeOnwerName { get; set; }

        public string TenantName { get; set; }

        public string ManagerName { get; set; }

        public string Reason { get; set; }

        public decimal Amount { get; set; }

        public PaymentStatus Status { get; set; }
    }
}
