using Application.Common.Services.EmailManager;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Infrastructure.EmailManager;


public class EmailService : IEmailService
{
    private readonly ILogger<EmailService> _logger;
    private readonly SmtpSettings _smtpSettings;

    public EmailService(ILogger<EmailService> logger, IOptions<SmtpSettings> smtpSettings)
    {
        _logger = logger;
        _smtpSettings = smtpSettings.Value;
    }

    public async Task SendEmailAsync(string recipientEmail, string subject, string htmlBody)
    {
        try
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(MailboxAddress.Parse(_smtpSettings.UserName));
            emailMessage.To.Add(MailboxAddress.Parse(recipientEmail));
            emailMessage.Subject = subject;

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = htmlBody
            };
            emailMessage.Body = bodyBuilder.ToMessageBody();

            using var smtpClient = new MailKit.Net.Smtp.SmtpClient();

            smtpClient.ServerCertificateValidationCallback = (s, c, h, e) => true;

            await smtpClient.ConnectAsync(_smtpSettings.Host, _smtpSettings.Port, MailKit.Security.SecureSocketOptions.SslOnConnect);
            await smtpClient.AuthenticateAsync(_smtpSettings.UserName, _smtpSettings.Password);
            await smtpClient.SendAsync(emailMessage);
            await smtpClient.DisconnectAsync(true);

            _logger.LogInformation("Email successfully sent to {RecipientEmail}", recipientEmail);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while sending email to {RecipientEmail}", recipientEmail);
            throw;
        }
    }

}
