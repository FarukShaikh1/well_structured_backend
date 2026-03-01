using FMS_Collection.Core.Common;
using FMS_Collection.Core.Interfaces;
using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Hosting;

namespace FMS_Collection.Infrastructure.Repositories
{
    public class NotificationSender : INotificationSender
    {
        string _emailPassword;
        string _senderEmail;
        private readonly IHostEnvironment _env;

        public NotificationSender(IHostEnvironment env)
        {
            _emailPassword = AppSettings.EmailPassword;
            _senderEmail = AppSettings.SenderEmail;
            _env = env;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body, bool isBodyHtml = true)
        {
            try
            {
                using var smtp = new SmtpClient(AppSettings.SmtpHost, AppSettings.SmtpPort) // Example: smtp.gmail.com, port 587
                {
                    Credentials = new NetworkCredential(_senderEmail, _emailPassword),
                    EnableSsl = true // use true for Gmail, Outlook, most providers
                };

                using var message = new MailMessage(AppSettings.SenderEmail, toEmail)
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = isBodyHtml // set false if plain text only
                };

                await smtp.SendMailAsync(message);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while sending email emailPassword is '{_emailPassword}' and error is: {ex.Message}", ex);
            }
        }

        public async Task<string> GetTemplateAsync(string templateName, Dictionary<string, string> values)
        {
            var path = Path.Combine(_env.ContentRootPath, "EmailTemplates", templateName);
            var template = await File.ReadAllTextAsync(path);

            foreach (var kv in values)
            {
                template = template.Replace($"{{{{{kv.Key}}}}}", kv.Value);
            }

            return template;
        }

        public Task SendSmsAsync(string toPhone, string message)
        {
            // TODO: integrate with SMS provider (Twilio, etc.). For now, no-op.
            return Task.CompletedTask;
        }
    }
}


