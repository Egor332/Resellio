using NotificationService.Models;

namespace NotificationService.Services.Abstractions
{
    public interface ICustomEmailSender
    {
        public Task<bool> SendEmailAsync(EmailMessageModel message);
    }
}
