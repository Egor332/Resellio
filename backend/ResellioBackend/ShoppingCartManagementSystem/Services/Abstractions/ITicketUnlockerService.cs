using ResellioBackend.Results;

namespace ResellioBackend.ShoppingCartManagementSystem.Services.Abstractions
{
    public interface ITicketUnlockerService
    {
        public Task<ResultBase> UnlockTicketAsync(int userId, Guid ticketId);
    }
}
