namespace RPM.Web.Areas.Management.Models.TransactionRequests
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using RPM.Data.Models;
    using RPM.Data.Models.Enums;
    using RPM.Services.Common.Models.City;
    using RPM.Services.Common.Models.Country;
    using RPM.Services.Management.Models;
    using static RPM.Common.GlobalConstants;

    public class OwnerTransactionRequestsCreateInputModel
    {
        public string Id { get; set; }

        [MaxLength(ReasonMaxLength)]
        public string Reason { get; set; }

        [Display(Name = "Recurring Schedule")]
        public RecurringScheduleType RecurringSchedule { get; set; }

        [Required]
        [Display(Name = "Is Recurring")]
        public bool IsRecurring { get; set; }

        [Display(Name = "Rental Location")]
        public int RentalId { get; set; }

        public IEnumerable<OwnerTransactionListOfRentalsServiceModel> RentalsList { get; set; }
    }
}
