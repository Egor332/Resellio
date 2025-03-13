namespace ResellioBackend.ShoppingCartManagementSystem.RedisServices.Abstractions
{
    public interface IRedisService
    {
        public Task<bool> InstantTicketLockAsync(Guid ticketId);
        public Task UnlockTicketAsync(Guid ticketId);
        public Task DeleteFromCartAsync(Guid ticketId, int userId);
        public Task AddToCartAsync(Guid ticketId, int userId);
    }
}
