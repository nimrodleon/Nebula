using System.Net.Mail;
using System.Net;
using Nebula.Common.Helpers;
using Microsoft.Extensions.Options;

namespace Nebula.Common;

public interface IEmailService
{
    Task SendEmailAsync(string fromEmail, string toEmail, string subject, string body);
}

public class EmailService : IEmailService
{
    private readonly SmtpClient _smtpClient;
    private readonly EmailSettings _settings;

    public EmailService(IOptions<EmailSettings> settings)
    {
        _settings = settings.Value;
        _smtpClient = new SmtpClient(_settings.SmtpServer)
        {
            Port = 587,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(_settings.SmtpUsername, _settings.SmtpPassword),
            EnableSsl = true
        };
    }

    public async Task SendEmailAsync(string fromEmail, string toEmail, string subject, string body)
    {
        var mailMessage = new MailMessage
        {
            From = new MailAddress(fromEmail),
            Subject = subject,
            Body = body,
            IsBodyHtml = true
        };

        mailMessage.To.Add(toEmail);
        await _smtpClient.SendMailAsync(mailMessage);
    }
}
