namespace RPM.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Text;

    using static RPM.Common.GlobalConstants;

    public class Document
    {
        public Document()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }

        [Required]
        [MaxLength(DocTitleMaxLength)]
        public string Title { get; set; }

        [Required]
        public string DocumentPath { get; set; }

        [Required]
        [ForeignKey(nameof(User))]
        public string UserId { get; set; }

        public User DocumeOwner { get; set; }
    }
}
