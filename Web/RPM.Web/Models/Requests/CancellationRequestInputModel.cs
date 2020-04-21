namespace RPM.Web.Models.Requests
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using RPM.Services.Common.Models.Rental;
    using static RPM.Common.GlobalConstants;

    public class CancellationRequestInputModel
    {
        public RentalInfoServiceModel RentalInfo { get; set; }

        [Required]
        [StringLength(MessageMaxLength, MinimumLength = MessageMinLength, ErrorMessage = "The message must have minimum {2} characters")]
        public string Message { get; set; }

        [Required]
        public IFormFile Document { get; set; }
    }
}
