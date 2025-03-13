using ResellioBackend.ShoppingCartManagementSystem.RedisRepositories.Abstractions;
using ResellioBackend.ShoppingCartManagementSystem.RedisServices.Abstractions;

namespace ResellioBackend.ShoppingCartManagementSystem.RedisServices.Implementations
{
    public class RedisService : IRedisService
    {
        private readonly ITicketRedisRepository _ticketRepository;
        private readonly ICartRedisRepository _cartRepository;
        private const double _baseLockTimeInMinutes = 10;

        public RedisService(ITicketRedisRepository ticketRepository, ICartRedisRepository cartRepository)
        {
            _ticketRepository = ticketRepository;
            _cartRepository = cartRepository;
        }

        public async Task AddToCartAsync(Guid ticketId, int userId)
        {
            var isCartExist = await _cartRepository.CheckCartForExistenceAsync(userId);
            if (isCartExist)
            {
                await _cartRepository.AddTicketToCartAsync(userId, ticketId);
                var cartTimeToLive = await _cartRepository.GetExpirationTimeAsync(userId);
                await _ticketRepository.SetExpirationTimeAsync(ticketId, cartTimeToLive.Value);
            }
            else
            {
                await _cartRepository.AddTicketToCartAsync(userId, ticketId);
                var ticketTimeToLive = await _ticketRepository.GetExpirationTimeAsync(ticketId);
                await _cartRepository.SetExpirationTimeAsync(userId, ticketTimeToLive.Value);
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

        public async Task<bool> InstantTicketLockAsync(Guid ticketId)
        {
            return await _ticketRepository.LockTicketAsync(ticketId, TimeSpan.FromMinutes(_baseLockTimeInMinutes));
        }

        public async Task UnlockTicketAsync(Guid ticketId)
        {
            await _ticketRepository.UnlockTicketAsync(ticketId);
        }
    }
}
