using MailKit.Net.Smtp;
using MimeKit;

namespace guest_house_management_backend.Services.Email
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;

        public EmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }    

        public async Task SendEmailASync(string toEmail, string subject, string message)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_configuration["EmailSettings:SenderEmail"]!));
            email.To.Add(MailboxAddress.Parse(toEmail));
            email.Subject = subject;
            email.Body = new TextPart("html") { Text = message };

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_configuration["EmailSettings:SmtpServer"], 587, MailKit.Security.SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_configuration["EmailSettings:SenderEmail"],_configuration["EmailSettings:Password"]);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);

        }
    }
}

