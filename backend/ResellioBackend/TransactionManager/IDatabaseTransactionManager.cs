using Microsoft.EntityFrameworkCore.Storage;

namespace ResellioBackend.TransactionManager
{
    public interface IDatabaseTransactionManager
    {
        public Task<IDbContextTransaction> BeginTransactionAsync();

        public Task CommitTransactionAsync(IDbContextTransaction transaction);

        public Task RollbackTransactionAsync(IDbContextTransaction transaction);
    }
}
