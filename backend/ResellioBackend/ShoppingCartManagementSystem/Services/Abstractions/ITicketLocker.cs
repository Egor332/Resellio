using ResellioBackend.Results;

namespace ResellioBackend.ShoppingCartManagementSystem.Services.Abstractions
{
    public interface ITicketLocker
    {
        public Task<ResultBase> LockTicket(int userId, Guid ticketId);
    }
}
