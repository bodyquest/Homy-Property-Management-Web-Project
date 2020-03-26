namespace RPM.Web.Areas.Management.Models.TransactionRequests
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using RPM.Data.Models.Enums;
    using RPM.Services.Management.Models;

    using static RPM.Common.GlobalConstants;

    public class OwnerTransactionRequestsCreateInputServiceModel
    {
        public string Id { get; set; }

        public string Reason { get; set; }

        public RecurringScheduleType RecurringSchedule { get; set; }

        public bool IsRecurring { get; set; }

        public int RentalId { get; set; }

        public IEnumerable<OwnerTransactionListOfRentalsServiceModel> RentalsList { get; set; }
    }
}
