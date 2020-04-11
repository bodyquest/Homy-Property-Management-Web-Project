namespace RPM.Services.Messaging
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using SendGrid;

    public class NullMessageSender : IEmailSender
    {
        // Task IEmailSender.SendEmailAsync(
        //    string email,
        //    string subject,
        //    string message)
        //{
        //    return Task.CompletedTask;
        //}

        public Task SendEmailAsync(
           string from,
           string fromName,
           string to,
           string subject,
           string htmlContent,
           IEnumerable<EmailAttachment> attachments = null)
        {
            return Task.CompletedTask;
        }

        public Task SendPlainEmailAsync(
            string from,
            string fromName,
            string to,
            string subject,
            string content,
            IEnumerable<EmailAttachment> attachments = null)
        {
            return Task.CompletedTask;
        }
    }
}
