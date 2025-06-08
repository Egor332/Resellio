
namespace ResellioBackend.TicketPurchaseSystem.RedisRepositories.Abstractions
{
    public interface ICartCacheRepository
    {
        public Task<bool> CheckCartForExistenceAsync(int userId);

        public Task AddTicketToCartAsync(int userId, Guid ticketId);

        public Task SetExpirationTimeAsync(int userId, TimeSpan timeSpan);

        public Task<TimeSpan?> GetExpirationTimeAsync(int userId);

        public Task DeleteTicketAsync(int userId, Guid ticketId);

        public Task DeleteCartAsync(int userId);

        public Task<IEnumerable<Guid>> GetAllTicketsAsync(int userId);

        public Task<long> GetCartLengthAsync(int userId);
    }
}
