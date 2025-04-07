using ResellioBackend.Results;

namespace ResellioBackend.TicketPurchaseSystem.Services.Abstractions
{
    public interface ITicketUnlockerService
    {
        public Task<ResultBase> UnlockTicketAsync(int userId, Guid ticketId);
    }
}
