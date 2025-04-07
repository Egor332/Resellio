using ResellioBackend.Results;

namespace ResellioBackend.TicketPurchaseSystem.Services.Abstractions
{
    public interface ITicketLockTimeService
    {
        public Task<ResultBase> ChangeLockExpirationTimeForTicketListAsync
            (List<Guid> ticketIds, DateTime newExpiration, int userId);
    }
}
