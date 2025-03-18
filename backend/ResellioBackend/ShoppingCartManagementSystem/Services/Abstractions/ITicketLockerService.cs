using ResellioBackend.Results;
using ResellioBackend.ShoppingCartManagementSystem.Results;

namespace ResellioBackend.ShoppingCartManagementSystem.Services.Abstractions
{
    public interface ITicketLockerService
    {
        public Task<TicketLockResult> LockTicketAsync(int userId, Guid ticketId);
    }
}
