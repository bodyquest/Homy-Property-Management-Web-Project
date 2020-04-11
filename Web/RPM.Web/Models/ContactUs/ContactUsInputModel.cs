namespace RPM.Web.Models.ContactUs
{
    using System.ComponentModel.DataAnnotations;

    using static RPM.Common.GlobalConstants;

    public class ContactUsInputModel
    {
        [Required]
        [StringLength(
            UsersNameMax,
            ErrorMessage = NameLengthError,
            MinimumLength = UsersNameMin)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Subject { get; set; }

        [Required]
        [StringLength(
            MessageMaxLength,
            ErrorMessage = MessageLengthError,
            MinimumLength = MessageMinLength)]
        public string Message { get; set; }
    }
}
