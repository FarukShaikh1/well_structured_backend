using System.Threading.Tasks;

namespace FMS_Collection.Core.Interfaces
{
    public interface INotificationSender
    {
        Task SendEmailAsync(string toEmail, string subject, string body, bool isBodyHtml=true);
        Task SendSmsAsync(string toPhone, string message);
        Task<string> GetTemplateAsync(string templateName, Dictionary<string, string> values);
    }
}


