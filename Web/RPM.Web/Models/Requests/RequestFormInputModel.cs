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
        public PropertyDetailsServiceModel PropertyDetails { get; set; }

        public User User { get; set; }

        [Required]
        [MinLength(MessageMinLength)]
        [MaxLength(MessageMaxLength)]
        public string Message { get; set; }

        [Required]
        public IFormFile Document { get; set; }
    }
}
