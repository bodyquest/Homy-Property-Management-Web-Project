namespace RPM.Web.Areas.Management.Models.TransactionRequests
{
    using RPM.Data.Models.Enums;

    using static RPM.Common.GlobalConstants;

    public class OwnerTransactionToRequestsCreateInputServiceModel
    {
        public string Id { get; set; }

        public string Reason { get; set; }

        public decimal Amount { get; set; }

        public RecurringScheduleType RecurringSchedule { get; set; }

        public bool IsRecurring { get; set; }

        public string HomeId { get; set; }
    }
}
