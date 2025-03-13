using ResellioBackend.Results;

namespace ResellioBackend.ShoppingCartManagementSystem.DatabaseServices.Abstractions
{
    public interface ITicketStatusService
    {
        public Task<ResultBase> LockTicketInDbAsync(Guid ticketId, DateTime newLockTime);
        public Task<ResultBase> UnlockTicketInDbAsync(Guid ticketId);
    }
}
