namespace guest_house_management_backend.Services.Email
{
    public interface IEmailSender
    {
        public Task SendEmailASync(string toEmail , string subject , string message);
    }
}
