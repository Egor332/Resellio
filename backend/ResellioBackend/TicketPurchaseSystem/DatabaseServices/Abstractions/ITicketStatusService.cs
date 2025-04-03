using ResellioBackend.EventManagementSystem.Models.Base;
using ResellioBackend.Results;

namespace ResellioBackend.TicketPurchaseSystem.DatabaseServices.Abstractions
{
    public interface ITicketStatusService
    {
        public Task<ResultBase> LockTicketInDbAsync(Guid ticketId, DateTime newLockTime);
        public Task<ResultBase> UnlockTicketInDbAsync(Ticket ticket);
    }
}
