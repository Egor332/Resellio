using ResellioBackend.Results;
using ResellioBackend.TicketPurchaseSystem.RedisRepositories.Abstractions;
using ResellioBackend.TicketPurchaseSystem.RedisServices.Abstractions;

namespace ResellioBackend.TicketPurchaseSystem.RedisServices.Implementations
{
    public class RedisService : IRedisService
    {
        private readonly ITicketRedisRepository _ticketRepository;
        private readonly ICartRedisRepository _cartRepository;

        public RedisService(ITicketRedisRepository ticketRepository, ICartRedisRepository cartRepository)
        {
            _ticketRepository = ticketRepository;
            _cartRepository = cartRepository;
        }

        public async Task<DateTime> AddToCartAsync(Guid ticketId, int userId)
        {
            var isCartExist = await _cartRepository.CheckCartForExistenceAsync(userId);
            if (isCartExist)
            {
                await _cartRepository.AddTicketToCartAsync(userId, ticketId);
                var cartTimeToLive = await _cartRepository.GetExpirationTimeAsync(userId);
                var cartExpirationTime = DateTime.UtcNow + cartTimeToLive;
                await _ticketRepository.SetExpirationTimeAsync(ticketId, cartTimeToLive.Value);
                return cartExpirationTime.Value;
            }
            else
            {
                await _cartRepository.AddTicketToCartAsync(userId, ticketId);
                var ticketTimeToLive = await _ticketRepository.GetExpirationTimeAsync(ticketId);
                var cartExpirationTime = DateTime.UtcNow + ticketTimeToLive;
                await _cartRepository.SetExpirationTimeAsync(userId, ticketTimeToLive.Value);
                return cartExpirationTime.Value;
            }
        }

        public async Task DeleteFromCartAsync(Guid ticketId, int userId)
        {
            await _cartRepository.DeleteTicketAsync(userId, ticketId);
            var cartSize = await _cartRepository.GetCartLengthAsync(userId);
            if (cartSize == 0)
            {
                await _cartRepository.DeleteCartAsync(userId);
            }
        }

        public async Task<bool> InstantTicketLockAsync(Guid ticketId, TimeSpan lockTime, int userId)
        {
            return await _ticketRepository.LockTicketAsync(ticketId, lockTime, userId);
        }

        public async Task<ResultBase> UnlockTicketAsync(Guid ticketId, int userId)
        {
            var previousLockerId = await _ticketRepository.GetUserIdAsync(ticketId);
            if (previousLockerId != null && userId != previousLockerId)
            {
                return new ResultBase
                {
                    Success = false,
                    Message = "This user can't unlock this ticket"
                };
            }
            await _ticketRepository.UnlockTicketAsync(ticketId);
            return new ResultBase
            {
                Success = true,
            };
        }

        public async Task<bool> ChangeExpirationTimeForTicketAsync(Guid ticketId, TimeSpan expirationTime, int userId)
        {
            var lockerId = await _ticketRepository.GetUserIdAsync(ticketId);
            if (lockerId != null && userId != lockerId)
            {
                return false;
            }
            var isExpirationSet = await _ticketRepository.SetExpirationTimeAsync(ticketId, expirationTime);
            if (!isExpirationSet)
            {
                var isNewLockSet = await _ticketRepository.LockTicketAsync(ticketId, expirationTime, userId);
                if (!isNewLockSet)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
