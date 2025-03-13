using ResellioBackend.Results;
using ResellioBackend.ShoppingCartManagementSystem.DatabaseServices.Abstractions;

namespace ResellioBackend.ShoppingCartManagementSystem.DatabaseServices.Implementations
{
    public class TicketStatusService : ITicketStatusService
    {        

        public Task<ResultBase> LockTicketInDbAsync(Guid ticketId, DateTime newLockTime)
        {
            throw new NotImplementedException();
        }

        public Task<ResultBase> UnlockTicketInDbAsync(Guid ticketId)
        {
            throw new NotImplementedException();
        }
    }
}
