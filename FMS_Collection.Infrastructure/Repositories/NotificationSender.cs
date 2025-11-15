using FMS_Collection.Core.Common;
using FMS_Collection.Core.Interfaces;
using System.Net;
using System.Net.Mail;
namespace FMS_Collection.Infrastructure.Repositories
{
    public class NotificationSender : INotificationSender
    {

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            try
            {
                using var smtp = new SmtpClient(AppSettings.SmtpHost, AppSettings.SmtpPort) // Example: smtp.gmail.com, port 587
                {
                    Credentials = new NetworkCredential(AppSettings.SenderEmail, AppSettings.EmailPassword),
                    EnableSsl = true // use true for Gmail, Outlook, most providers
                };

                using var message = new MailMessage(AppSettings.SenderEmail, toEmail)
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true // set false if plain text only
                };

                await smtp.SendMailAsync(message);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while sending email: {ex.Message}", ex);
            }
        }


        public Task SendSmsAsync(string toPhone, string message)
        {
            // TODO: integrate with SMS provider (Twilio, etc.). For now, no-op.
            return Task.CompletedTask;
        }
    }
}


