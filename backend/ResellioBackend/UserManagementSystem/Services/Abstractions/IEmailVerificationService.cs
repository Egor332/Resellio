using ResellioBackend.UserManagementSystem.Models.Base;

namespace ResellioBackend.UserManagementSystem.Services.Abstractions
{
    public interface IEmailVerificationService
    {
        public Task CreateAndSendVerificationEmailAsync(UserBase user);
    }
}
