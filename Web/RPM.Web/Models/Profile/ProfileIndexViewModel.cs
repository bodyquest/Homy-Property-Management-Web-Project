namespace RPM.Web.Models.Profile
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using RPM.Services.Common.Models.Profile;
    using RPM.Services.Common.Models.Rental;

    public class ProfileIndexViewModel
    {
        public ProfileIndexViewModel()
        {
            this.Payments = new HashSet<UserPaymentListServiceModel>();
        }

        public IEnumerable<UserPaymentListServiceModel> Payments { get; set; }

        public IEnumerable<UserRentalListServiceModel> Rentals { get; set; }
    }
}
