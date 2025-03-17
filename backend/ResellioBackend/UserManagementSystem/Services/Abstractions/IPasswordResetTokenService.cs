using ResellioBackend.UserManagementSystem.Results;

namespace ResellioBackend.UserManagementSystem.Services.Abstractions
{
    public interface IPasswordResetTokenService
    {
        public Task<string> CreateTokenWithDatabaseRecordAsync(int userId);
        public VerifyResetPasswordTokenResult VerifyPasswordResetToken(string token);
    }
}
