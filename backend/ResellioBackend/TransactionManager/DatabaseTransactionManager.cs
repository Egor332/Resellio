using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Identity.Client;

namespace ResellioBackend.TransactionManager
{
    public class DatabaseTransactionManager : IDatabaseTransactionManager
    {
        private readonly ResellioDbContext _context;

        public DatabaseTransactionManager(ResellioDbContext context) 
        {
            _context = context;
        }
        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync(IDbContextTransaction transaction)
        {
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
        }

        public async Task RollbackTransactionAsync(IDbContextTransaction transaction)
        {
            await transaction.RollbackAsync();
        }
    }
}
