namespace ResellioBackend.UserManagementSystem.Services.Abstractions
{
    public interface ICustomEmailSender
    {
        public Task<bool> SendEmailAsync(string email, string subject, string plainTextContent, string htmlContent);
    }
}
