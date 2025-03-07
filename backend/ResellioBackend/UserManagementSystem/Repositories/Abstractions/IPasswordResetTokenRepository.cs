using ResellioBackend.UserManagementSystem.Models.Tokens;

namespace ResellioBackend.UserManagementSystem.Repositories.Abstractions
{
    public interface IPasswordResetTokenRepository
    {
        public Task<PasswordResetTokenInfo?> GetByIdAsync(Guid id);
        public Task AddTokenAsync(PasswordResetTokenInfo token);
        public Task DeleteTokenAsync(PasswordResetTokenInfo token);
        public Task<List<PasswordResetTokenInfo>?> GetUserTokensAsync(int ownerId);
    }
}
