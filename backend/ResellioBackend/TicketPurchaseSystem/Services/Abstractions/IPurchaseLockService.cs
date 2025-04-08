using ResellioBackend.Results;

namespace ResellioBackend.TicketPurchaseSystem.Services.Abstractions
{
    public interface IPurchaseLockService
    {
        public Task<ResultBase> EnsureEnoughLockTimeForPurchaseAsync(int userId);
        public Task<ResultBase> GetAddedTimeBackAsync(List<Guid> ticketIds, int userId);
    }
}
