using ResellioBackend.Results;
using ResellioBackend.ShoppingCartManagementSystem.DatabaseServices.Abstractions;
using ResellioBackend.ShoppingCartManagementSystem.RedisServices.Abstractions;
using ResellioBackend.ShoppingCartManagementSystem.Results;
using ResellioBackend.ShoppingCartManagementSystem.Services.Abstractions;

namespace ResellioBackend.ShoppingCartManagementSystem.Services.Implementations
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
            var isLockSuccess = await _redisService.InstantTicketLockAsync(ticketId, TimeSpan.FromMinutes(_defaultLockMinutes));
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
                await _redisService.UnlockTicketAsync(ticketId);
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
