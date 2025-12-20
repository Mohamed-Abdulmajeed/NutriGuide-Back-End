using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Logging;

namespace NutriGuide.Services
{
    public class EmailService:IEmailService
    {
        private readonly IConfiguration _config;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IConfiguration config, ILogger<EmailService> logger)
        {
            _config = config;
            _logger = logger;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string message) 
        {
            var smtpServer = _config["Smtp:Server"];
            var port = int.Parse(_config["Smtp:Port"]);
            var senderEmail = _config["Smtp:SenderEmail"];
            var password = _config["Smtp:Password"];
            var client = new SmtpClient(smtpServer, port)
            {
                Credentials = new NetworkCredential(senderEmail, password),
                EnableSsl = true
            };
            var mail = new MailMessage(senderEmail, toEmail)
            {
                Subject = subject,
                Body = message
            };

            try
            {
                await client.SendMailAsync(mail);
            }
            catch (Exception ex)
            {
                // Log full exception for diagnosis (includes native inner exceptions)
                _logger.LogError(ex, "Failed to send email to {Email}", toEmail);
                throw;
            }

        }

    }
}
