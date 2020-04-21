namespace RPM.Web.Models.Requests
{
    using System.ComponentModel.DataAnnotations;
    using Microsoft.AspNetCore.Http;
    using RPM.Services.Common.Models.Listing;

    using static RPM.Common.GlobalConstants;

    public class ManageCancellationRequestInputModel
    {
        public ManagedHomeInfoServiceModel HomeInfo { get; set; }

        [Required]
        [StringLength(MessageMaxLength, MinimumLength = MessageMinLength, ErrorMessage = "The message must have minimum {2} characters")]
        public string Message { get; set; }

        [Required]
        public IFormFile Document { get; set; }
    }
}
