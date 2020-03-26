namespace RPM.Web.Areas.Management.Models.Requests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using RPM.Data.Models;
    using RPM.Services.Management.Models;

    public class OwnerTransactionRequestsWithDetailsViewModel
    {
        public IEnumerable<OwnerAllTransactionRequestsServiceModel>
            TransactionRequests { get; set; }
    }
}
