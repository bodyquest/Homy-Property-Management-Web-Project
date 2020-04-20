using RPM.Data.Models.Enums;

namespace RPM.Services.Management.Models
{
    public class OwnerRequestDetailsServiceModel
    {

        public string Id { get; set; }

        public string Date { get; set; }

        public string UserFirstName { get; set; }

        public string UserLastName { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string RequestType { get; set; }

        public RequestStatus Status { get; set; }

        public string Message { get; set; }

        public string About { get; set; }

        public byte[] Document { get; set; }

        public OwnerRequestListingDetailsServiceModel HomeInfo { get; set; }
    }
}
