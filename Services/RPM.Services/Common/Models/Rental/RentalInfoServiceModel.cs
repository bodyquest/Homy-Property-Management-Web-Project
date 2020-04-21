namespace RPM.Services.Common.Models.Rental
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class RentalInfoServiceModel
    {
        public int Id { get; set; }

        public string RentalDate { get; set; }

        public string Owner { get; set; }

        public string TenantId { get; set; }

        public decimal Price { get; set; }

        public string Image { get; set; }
    }
}
