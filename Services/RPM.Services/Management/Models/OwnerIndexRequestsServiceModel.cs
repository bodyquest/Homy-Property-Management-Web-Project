namespace RPM.Services.Management.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class OwnerIndexRequestsServiceModel
    {
        public string Id { get; set; }

        public DateTime Date { get; set; }

        public string Type { get; set; }

        public string Location { get; set; }
    }
}
