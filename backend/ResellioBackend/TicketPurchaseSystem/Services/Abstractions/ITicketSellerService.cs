using ResellioBackend.EventManagementSystem.Models.Base;
using ResellioBackend.Results;
using ResellioBackend.UserManagementSystem.Models.Users;

namespace ResellioBackend.TicketPurchaseSystem.Services.Abstractions
{
    public interface ITicketSellerService
    {
        public Task<ResultBase> TryMarkTicketsAsSoldAsync(List<Guid> ticketIds, Customer buyer);
    }
}
