namespace RPM.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Rental
    {
        public Rental()
        {
            this.TransactionRequests = new HashSet<TransactionRequest>();
            this.Payments = new HashSet<Payment>();
        }

        public int Id { get; set; }

        public DateTime RentDate { get; set; }

        [ForeignKey(nameof(Home))]
        public string HomeId { get; set; }
        public virtual Home Home { get; set; }

        public string TenantId { get; set; }

        [ForeignKey("TenantId")]
        public virtual User Tenant { get; set; }

        public string ManagerId { get; set; }

        [ForeignKey("ManagerId")]
        public virtual User Manager { get; set; }

        public int? Duration { get; set; }

        [ForeignKey(nameof(Contract))]
        public string ContractId { get; set; }
        public virtual Contract Contract { get; set; }

        public ICollection<TransactionRequest> TransactionRequests { get; set; }
        public ICollection<Payment> Payments { get; set; }
    }
}
