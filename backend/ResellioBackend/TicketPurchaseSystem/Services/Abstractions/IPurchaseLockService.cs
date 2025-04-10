using ResellioBackend.Results;

namespace ResellioBackend.TicketPurchaseSystem.Services.Abstractions
{
    public interface IPurchaseLockService
    {
        public Task<ResultBase> EnsureEnoughLockTimeForPurchaseAsync(int userId);
        public Task RollbackAddedTimeAsync(List<Guid> ticketIds, int userId);
    }
}
