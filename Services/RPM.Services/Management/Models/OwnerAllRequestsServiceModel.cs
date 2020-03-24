namespace RPM.Services.Management.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class OwnerAllRequestsServiceModel
    {
        public string Id { get; set; }

        public string Type { get; set; }

        public string Date { get; set; }

        public string FullName { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string Status { get; set; }

        public string Location { get; set; }
    }
}
