using ResellioBackend.Results;

namespace ResellioBackend.ShoppingCartManagementSystem.RedisServices.Abstractions
{
    public interface IRedisService
    {
        public Task<bool> InstantTicketLockAsync(Guid ticketId, TimeSpan lockTime, int userId);
        public Task<ResultBase> UnlockTicketAsync(Guid ticketId, int userId);
        public Task DeleteFromCartAsync(Guid ticketId, int userId);
        public Task<DateTime> AddToCartAsync(Guid ticketId, int userId);
    }
}
