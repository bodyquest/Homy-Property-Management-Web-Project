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
        public Home Home { get; set; }

        [ForeignKey(nameof(User))]
        public string TenantId { get; set; }
        public User Tenant { get; set; }

        public virtual ICollection<Document> Documents { get; set; }
    }
}
