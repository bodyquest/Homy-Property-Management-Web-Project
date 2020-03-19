namespace RPM.Services.Common.Models.Request
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;
    using Microsoft.AspNetCore.Http;
    using RPM.Services.Common.Models.Listing;
    using RPM.Services.Common.Models.User;

    using static RPM.Common.GlobalConstants;

    public class RequestFormServiceModel
    {
        public PropertyDetailsServiceModel PropertyDetails { get; set; }
    }
}
