using ResellioBackend.EventManagementSystem.Models.Base;
using ResellioBackend.Results;

namespace ResellioBackend.TicketPurchaseSystem.Services.Abstractions
{
    public interface ITicketSellerService
    {
        public Task<ResultBase> TryMarkTicketsAsSoledAsync(List<Ticket> tickets);
    }
}
