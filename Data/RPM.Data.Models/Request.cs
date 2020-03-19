namespace RPM.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Text;
    using Microsoft.AspNetCore.Http;
    using RPM.Data.Models.Enums;

    using static RPM.Common.GlobalConstants;

    public class Request
    {
        public Request()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }

        public DateTime Date { get; set; } = DateTime.UtcNow;

        [Required]
        public RequestType Type { get; set; }

        [Required]
        [ForeignKey(nameof(User))]
        public string UserId { get; set; }
        public User User { get; set; }

        [Required]
        [ForeignKey(nameof(Home))]
        public string HomeId { get; set; }
        public Home Home { get; set; }

        [Required]
        [MaxLength(RequestDocumentMaxSize)]
        public byte[] Document { get; set; }
    }
}
