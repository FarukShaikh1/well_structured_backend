using FMS_Collection.Core.Common;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;
using FMS_Collection.Core.Common;
using FMS_Collection.Core.Interfaces;
using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Hosting;

public class EmailService 
{
    private readonly IEmailRepository _emailRepository;
    private readonly IConfiguration _configuration;

    public EmailService(IEmailRepository emailRepository,IConfiguration configuration)
    {
        _emailRepository = emailRepository;
        _configuration = configuration;
    }

    public async Task<bool> SendAsync(string to,string templateCode,Dictionary<string, string>? parameters = null,CancellationToken cancellationToken = default)
    {
        bool success = false;
        try
        {
            var template = await _emailRepository.GetByCodeAsync(templateCode, cancellationToken);
            if (template == null)
            {
                throw new InvalidOperationException($"Email template '{templateCode}' was not found or is inactive.");
            }
            var subject = ReplacePlaceholders(template.Subject, parameters);

            var body = ReplacePlaceholders(template.BodyHtml, parameters);

            await SendEmailAsync(to, subject, body, cancellationToken);
            return true;
        }
        catch (Exception ex) 
        {
            return false;
        }
    }

    private async Task SendEmailAsync(string to,string subject,string body,CancellationToken cancellationToken)
    {
        var smtpHost = AppSettings.SmtpHost;
        var smtpPort = AppSettings.SmtpPort;

        var _emailPassword = AppSettings.EmailPassword;
        var _senderEmail = AppSettings.SenderEmail;
        var fromName = AppSettings.SenderEmail;

        using var message = new MailMessage();

        message.From = new MailAddress(_senderEmail!,fromName);
        message.To.Add(to);
        message.Subject = subject;
        message.Body = body;
        message.IsBodyHtml = true;

        using var smtpClient = new SmtpClient(smtpHost,smtpPort);
        smtpClient.EnableSsl = true;
        smtpClient.Credentials = new NetworkCredential(_senderEmail,_emailPassword);

        await smtpClient.SendMailAsync(message, cancellationToken);


        //using var smtp = new SmtpClient(AppSettings.SmtpHost, AppSettings.SmtpPort) // Example: smtp.gmail.com, port 587
        //{
        //    Credentials = new NetworkCredential(_senderEmail, _emailPassword),
        //    EnableSsl = true // use true for Gmail, Outlook, most providers
        //};

        //using var message1 = new MailMessage(_senderEmail!, toEmail)
        //{
        //    Subject = subject,
        //    Body = body,
        //    IsBodyHtml = true // set false if plain text only
        //};

        //await smtp.SendMailAsync(message);

    }

    private static string ReplacePlaceholders(string content,Dictionary<string, string>? parameters)
    {
        if (parameters == null || parameters.Count == 0)
            return content;

        foreach (var parameter in parameters)
        {
            content = content.Replace("{{" + parameter.Key + "}}",parameter.Value ?? string.Empty,StringComparison.OrdinalIgnoreCase);
        }

        return content;
    }
}