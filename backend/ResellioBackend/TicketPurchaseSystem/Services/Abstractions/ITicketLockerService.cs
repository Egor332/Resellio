using ResellioBackend.Results;
using ResellioBackend.TicketPurchaseSystem.Results;

namespace ResellioBackend.TicketPurchaseSystem.Services.Abstractions
{
    public interface ITicketLockerService
    {
        public Task<TicketLockResult> LockTicketAsync(int userId, Guid ticketId);
    }
}
