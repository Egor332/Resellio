using ResellioBackend.Results;
using ResellioBackend.TicketPurchaseSystem.Services.Abstractions;

namespace ResellioBackend.TicketPurchaseSystem.Services.Implementations
{
    public class TicketLockTimeService : ITicketLockTimeService
    {
        public async Task<ResultBase> ChangeLockExpirationTimeForTicketListAsync
            (List<Guid> ticketIds, DateTime newExpiration, int userId)
        {
            foreach (var ticketId in ticketIds)
            {

            }
            throw new NotImplementedException();
        } 
    }
}
