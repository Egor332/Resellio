using Microsoft.EntityFrameworkCore;
using ResellioBackend.UserManagementSystem.Models.Tokens;
using ResellioBackend.UserManagementSystem.Repositories.Abstractions;

namespace ResellioBackend.UserManagementSystem.Repositories.Implementations
{
    public class PasswordResetTokenRepository : IPasswordResetTokenRepository
    {
        private readonly ResellioDbContext _context;
        private readonly DbSet<PasswordResetTokenInfo> _passwordResetTokens;

        public PasswordResetTokenRepository(ResellioDbContext context)
        {
            _context = context;
            _passwordResetTokens = context.PasswordResetTokens;
        }

        public async Task AddTokenAsync(PasswordResetTokenInfo token)
        {
            await _passwordResetTokens.AddAsync(token);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteTokenAsync(PasswordResetTokenInfo token)
        {
            _passwordResetTokens.Remove(token);
            await _context.SaveChangesAsync();
        }

        public async Task<PasswordResetTokenInfo?> GetByIdAsync(Guid id)
        {
            var tokenInfo = await _passwordResetTokens.FirstOrDefaultAsync(t => t.Id == id);
            return tokenInfo;
        }

        public async Task<List<PasswordResetTokenInfo>?> GetUserTokensAsync(int ownerId)
        {
            var tokensInfoList = await _passwordResetTokens.Where(t => t.OwnerId == ownerId).ToListAsync();
            return tokensInfoList;
        }
    }
}
