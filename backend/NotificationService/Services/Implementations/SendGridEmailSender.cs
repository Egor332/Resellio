using NotificationService.Models;
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

        public async Task<bool> SendEmailAsync(EmailMessageModel message)
        {
            var client = ClientFactory();

            var from = new EmailAddress(_senderEmail, "Resellio");
            var to = new EmailAddress(message.Email, message.Email);

            var msg = MailHelper.CreateSingleEmail(from, to, message.Subject, message.PlainTextContent, message.HtmlContent);
            msg.Subject = message.Subject; // does not work without this line, may be because of some conflict in CreateSingleEmail
            var response = await client.SendEmailAsync(msg);

            return response.IsSuccessStatusCode;
        }
    }
}
