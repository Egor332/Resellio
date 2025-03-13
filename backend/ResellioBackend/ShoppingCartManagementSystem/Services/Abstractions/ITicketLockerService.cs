using ResellioBackend.Results;

namespace ResellioBackend.ShoppingCartManagementSystem.Services.Abstractions
{
    public interface ITicketLockerService
    {
        public Task<ResultBase> LockTicket(int userId, Guid ticketId);
    }
}
