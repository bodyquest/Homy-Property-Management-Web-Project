namespace RPM.Web.Models.Profile
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using RPM.Services.Common.Models.Profile;

    public class ProfileIndexViewModel
    {
        public IEnumerable<UserPaymentListServiceModel> Payments { get; set; }
    }
}
