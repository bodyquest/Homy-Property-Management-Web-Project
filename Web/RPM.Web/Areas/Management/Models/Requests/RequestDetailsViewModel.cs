namespace RPM.Web.Areas.Management.Models.Requests
{
    using Microsoft.AspNetCore.Http;
    using RPM.Data.Models.Enums;
    using RPM.Services.Management.Models;
    using System.ComponentModel.DataAnnotations;

    public class RequestDetailsViewModel
    {
        public string Id { get; set; }

        public string Date { get; set; }

        public string UserFirstName { get; set; }

        public string UserLastName { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string RequestType { get; set; }

        public string Message { get; set; }

        public string About { get; set; }

        public RequestStatus Status { get; set; }

        public byte[] Document { get; set; }

        [Required]
        public IFormFile Contract { get; set; }

        public OwnerRequestListingDetailsServiceModel HomeInfo { get; set; }
    }
}
