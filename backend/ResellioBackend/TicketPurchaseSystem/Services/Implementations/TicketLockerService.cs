using ResellioBackend.Results;
using ResellioBackend.TicketPurchaseSystem.DatabaseServices.Abstractions;
using ResellioBackend.TicketPurchaseSystem.RedisServices.Abstractions;
using ResellioBackend.TicketPurchaseSystem.Results;
using ResellioBackend.TicketPurchaseSystem.Services.Abstractions;

namespace ResellioBackend.TicketPurchaseSystem.Services.Implementations
{
    public class TicketLockerService : ITicketLockerService
    {
        private readonly ITicketStatusService _ticketStatusService;
        private readonly IRedisService _redisService;
        private const double _defaultLockMinutes = 10;

        public TicketLockerService(ITicketStatusService ticketStatusService, IRedisService redisService)
        {
            _ticketStatusService = ticketStatusService;
            _redisService = redisService;
        }

        public async Task<TicketLockResult> LockTicketAsync(int userId, Guid ticketId)
        {
            var isLockSuccess = await _redisService.InstantTicketLockAsync(ticketId, TimeSpan.FromMinutes(_defaultLockMinutes), userId);
            if (!isLockSuccess)
            {
                return new TicketLockResult()
                {
                    Success = false,
                    Message = "Ticket was already locked"
                };
            }

            var cartExpirationTime = await _redisService.AddToCartAsync(ticketId, userId);

            var databaseLockResult = await _ticketStatusService.LockTicketInDbAsync(ticketId, cartExpirationTime);
            if (!databaseLockResult.Success)
            {
                await _redisService.DeleteFromCartAsync(ticketId, userId);
                await _redisService.UnlockTicketAsync(ticketId, userId);
                return new TicketLockResult()
                {
                    Success = false,
                    Message = databaseLockResult.Message
                };
            }

            return new TicketLockResult()
            {
                Success = true,
                Message = "Ticked successfully locked",
                TicketId = ticketId,
                ExpirationTime = cartExpirationTime
            };
        }
    }
}
