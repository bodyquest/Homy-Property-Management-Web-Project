namespace RPM.Services.Management.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class OwnerAllRentalsServiceModel
    {
        public int Id { get; set; }

        public string Date { get; set; }

        public int? Duration { get; set; }

        public string Location { get; set; }

        public string FullName { get; set; }

        public string Username { get; set; }

        public string ManagerName { get; set; }

        public decimal Price { get; set; }

        public string HomeCategory { get; set; }

        public string HomeImage { get; set; }
    }
}
