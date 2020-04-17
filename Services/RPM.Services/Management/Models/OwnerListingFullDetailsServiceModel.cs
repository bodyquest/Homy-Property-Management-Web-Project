namespace RPM.Services.Management.Models
{
    using RPM.Data.Models.Enums;

    public class OwnerListingFullDetailsServiceModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string City { get; set; }

        public string Address { get; set; }

        public string Country { get; set; }

        public string Description { get; set; }

        public string ManagerFullName { get; set; }

        public decimal Price { get; set; }

        public HomeStatus Status { get; set; }

        public HomeCategory Category { get; set; }

        public string Image { get; set; }

        public OwnerRentalInfoServiceModel RentalInfo { get; set; }
    }
}