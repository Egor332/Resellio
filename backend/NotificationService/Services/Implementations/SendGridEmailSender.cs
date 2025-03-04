using NotificationService.Services.Abstractions;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Net.Mail;

namespace NotificationService.Services.Implementations
{
    public class SendGridEmailSender : ICustomEmailSender
    {
        private readonly string _apiKey;
        private readonly string _senderEmail;
        public Func<ISendGridClient> ClientFactory { get; set; }

        public SendGridEmailSender(IConfiguration configuration)
        {
            _apiKey = configuration["SendGridParameters:SendGridKey"];
            _senderEmail = configuration["SendGridParameters:CompanyEmail"];

            ClientFactory = () => new SendGridClient(_apiKey);
        }

        public async Task<bool> SendEmailAsync(string email, string subject, string plainTextContent, string htmlContent)
        {
            var client = ClientFactory();

            var from = new EmailAddress(_senderEmail, "Resellio");
            var to = new EmailAddress(email, email);

            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            msg.Subject = subject; // does not work without this line, may be because of some conflict in CreateSingleEmail
            var response = await client.SendEmailAsync(msg);

            return response.IsSuccessStatusCode;
        }
    }
}
