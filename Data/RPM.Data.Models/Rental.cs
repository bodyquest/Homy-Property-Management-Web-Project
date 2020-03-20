namespace RPM.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Rental
    {
        public int Id { get; set; }

        public DateTime RentDate { get; set; }

        [ForeignKey(nameof(Home))]
        public string HomeId { get; set; }
        public virtual Home Home { get; set; }

        [ForeignKey(nameof(User))]
        public string TenantId { get; set; }
        public virtual User Tenant { get; set; }

        [ForeignKey(nameof(Contract))]
        public string ContractId { get; set; }
        public virtual Contract Contract { get; set; }
    }
}
