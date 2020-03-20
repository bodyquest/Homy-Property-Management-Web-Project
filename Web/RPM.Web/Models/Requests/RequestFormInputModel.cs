namespace RPM.Web.Models.Requests
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using RPM.Data.Models;
    using RPM.Services.Common.Models.Listing;
    using RPM.Services.Common.Models.Request;
    using static RPM.Common.GlobalConstants;

    public class RequestFormInputModel
    {
        public string Id { get; set; }

        public PropertyDetailsServiceModel PropertyDetails { get; set; }

        public User User { get; set; }

        [Phone]
        [Required]
        [Display(Name = "Phone")]
        public string PhoneNumber { get; set; }

        [Required]
        [MaxLength(AboutMaxLength)]
        public string About { get; set; }

        [Required]
        [StringLength(MessageMaxLength, MinimumLength = MessageMinLength, ErrorMessage = "The message must have minimum {2} characters")]
        public string Message { get; set; }

        [Required]
        public IFormFile Document { get; set; }
    }
}
