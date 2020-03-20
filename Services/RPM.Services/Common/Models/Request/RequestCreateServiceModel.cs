namespace RPM.Services.Common.Models.Request
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using RPM.Data.Models.Enums;

    public class RequestCreateServiceModel
    {
        public DateTime Date { get; set; }

        public RequestType Type { get; set; }

        public string UserId { get; set; }

        public string HomeId { get; set; }

        public byte[] Document { get; set; }
    }
}
