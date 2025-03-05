using ResellioBackend.UserManagementSystem.Results;

namespace ResellioBackend.UserManagementSystem.Services.Abstractions
{
    public interface IEmailVerificationTokenService
    {
        public string GetEmailVerificationToken(int id);
        public ValidateEmailVerificationTokenResult ValidateEmailVerificationToken(string emailVerificationToken);
    }
}
