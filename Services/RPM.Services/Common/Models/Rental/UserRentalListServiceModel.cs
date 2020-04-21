namespace RPM.Services.Common.Models.Rental
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class UserRentalListServiceModel
    {
        public int Id { get; set; }

        public string Date { get; set; }

        public string OwnerFullName { get; set; }

        public string Location { get; set; }

        public decimal Price { get; set; }
    }
}
