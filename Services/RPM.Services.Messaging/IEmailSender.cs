namespace RPM.Services.Messaging
{
    using SendGrid;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IEmailSender
    {
        //Task SendEmailAsync(
        //    string email,
        //    string subject,
        //    string message);

        Task SendEmailAsync(
            string from,
            string fromName,
            string to,
            string subject,
            string htmlContent,
            IEnumerable<EmailAttachment> attachments = null);

        Task SendPlainEmailAsync(
            string from,
            string fromName,
            string to,
            string subject,
            string content,
            IEnumerable<EmailAttachment> attachments = null);
    }
}
