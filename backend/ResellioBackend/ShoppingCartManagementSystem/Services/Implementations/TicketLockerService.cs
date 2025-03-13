using ResellioBackend.Results;
using ResellioBackend.ShoppingCartManagementSystem.DatabaseServices.Abstractions;
using ResellioBackend.ShoppingCartManagementSystem.RedisServices.Abstractions;
using ResellioBackend.ShoppingCartManagementSystem.Services.Abstractions;

namespace ResellioBackend.ShoppingCartManagementSystem.Services.Implementations
{
    public class TicketLockerService : ITicketLockerService
    {
        private readonly ITicketStatusService _ticketStatusService;
        private readonly IRedisService _redisService;

        public TicketLockerService(ITicketStatusService ticketStatusService, IRedisService redisService)
        {
            _ticketStatusService = ticketStatusService;
            _redisService = redisService;
        }

        public Task<ResultBase> LockTicket(int userId, Guid ticketId)
        {
            var lockResult = _redisService.InstantTicketLockAsync(ticketId);
        }
    }
}
