using ResellioBackend.Services.Abstractions;
using SendGrid.Helpers.Mail;
using SendGrid;

namespace ResellioBackend.Services.Implementations
{
    public class SendGridEmailSender : ICustomEmailSender
    {
        private readonly string _apiKey;
        private readonly string _senderEmail;
        public SendGridEmailSender(IConfiguration configuration)
        {
            _apiKey = configuration["SendGridParameters:SendGridKey"];
            _senderEmail = configuration["SendGridParameters:CompanyEmail"];
        }

        public async Task<bool> SendEmailAsync(string email, string subject, string plainTextContent, string htmlContent)
        {
            var client = new SendGridClient(_apiKey);

            var from = new EmailAddress(_senderEmail, "Resellio");
            var to = new EmailAddress(email, email);

            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);

            return response.IsSuccessStatusCode;
        }
    }
}
