namespace RPM.Web.Areas.Management.Models.TransactionRequests
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using RPM.Data.Models.Enums;
    using RPM.Services.Common.Models.City;
    using RPM.Services.Common.Models.Country;
    using RPM.Services.Management.Models;
    using static RPM.Common.GlobalConstants;

    public class OwnerTransactionPaymentsCreateInputModel
    {
        public string Id { get; set; }

        [MaxLength(ReasonMaxLength)]
        public string Reason { get; set; }

        [Display(Name = "Recurring Schedule")]
        public RecurringScheduleType RecurringSchedule { get; set; }

        [Required]
        [Display(Name = "Is Recurring")]
        public bool IsRecurring { get; set; }

        [Display(Name = "Managed home Location")]
        public string HomeId { get; set; }

        public IEnumerable<OwnerTransactionListOfManagedHomesServiceModel> ManagedHomesList { get; set; }
    }
}
