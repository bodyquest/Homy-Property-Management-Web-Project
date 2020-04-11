namespace RPM.Services.Common.Models.Listing
{
    using RPM.Data.Models.Enums;

    public class ManagerDashboardPropertiesServiceModel
    {
        public string Id { get; set; }

        public string OwnerName { get; set; }

        public string TenantName { get; set; }

        public string City { get; set; }

        public string Address { get; set; }

        public HomeStatus Status { get; set; }

        public HomeCategory Category { get; set; }
    }
}
