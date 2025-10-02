using System.Threading.Tasks;

namespace FMS_Collection.Core.Interfaces
{
    public interface INotificationSender
    {
        Task SendEmailAsync(string toEmail, string subject, string body);
        Task SendSmsAsync(string toPhone, string message);
    }
}


