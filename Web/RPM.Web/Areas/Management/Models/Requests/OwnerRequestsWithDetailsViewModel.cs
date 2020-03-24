namespace RPM.Web.Areas.Management.Models.Requests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using RPM.Services.Management.Models;

    public class OwnerRequestsWithDetailsViewModel
    {
        public IEnumerable<OwnerAllRequestsServiceModel> Requests { get; set; }
    }
}
