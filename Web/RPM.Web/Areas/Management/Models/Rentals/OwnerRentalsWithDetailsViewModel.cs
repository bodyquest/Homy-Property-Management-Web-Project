namespace RPM.Web.Areas.Management.Models.Rentals
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using RPM.Services.Management.Models;

    public class OwnerRentalsWithDetailsViewModel
    {
        public IEnumerable<OwnerAllRentalsServiceModel> Rentals { get; set; }
    }
}
