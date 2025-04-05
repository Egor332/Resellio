using ResellioBackend.EventManagementSystem.Models.Base;
using ResellioBackend.Results;
using ResellioBackend.UserManagementSystem.Models.Base;
using ResellioBackend.UserManagementSystem.Models.Users;

namespace ResellioBackend.TicketPurchaseSystem.DatabaseServices.Abstractions
{
    public interface ITicketStatusService
    {
        public Task<ResultBase> LockTicketInDbAsync(Guid ticketId, DateTime newLockTime);
        public Task<ResultBase> UnlockTicketInDbAsync(Ticket ticket);
        public Task<ResultBase> TryMarkAsSoledAsync(Guid ticketId, Customer buyer);
    }
}
