using ResellioBackend.TicketPurchaseSystem.RedisRepositories.Abstractions;
using StackExchange.Redis;

namespace ResellioBackend.TicketPurchaseSystem.RedisRepositories.Implementations
{
    public class CartRedisRepository : ICartRedisRepository
    {
        private readonly IDatabase _redisDb;

        public CartRedisRepository(IConnectionMultiplexer connectionMultiplexer)
        {
            _redisDb = connectionMultiplexer.GetDatabase();
        }

        public async Task<bool> CheckCartForExistenceAsync(int userId)
        {
            return await _redisDb.KeyExistsAsync($"cart:{userId}");
        }

        public async Task AddTicketToCartAsync(int userId, Guid ticketId)
        {
            await _redisDb.SetAddAsync($"cart:{userId}", ticketId.ToString());
        }

        public async Task SetExpirationTimeAsync(int userId, TimeSpan timeSpan)
        {
            await _redisDb.KeyExpireAsync($"cart:{userId}", timeSpan);
        }

        public async Task<TimeSpan?> GetExpirationTimeAsync(int userId)
        {
            return await _redisDb.KeyTimeToLiveAsync($"cart:{userId}");
        }

        public async Task DeleteTicketAsync(int userId, Guid ticketId)
        {
            await _redisDb.SetRemoveAsync($"cart:{userId}", ticketId.ToString());
        }

        public async Task DeleteCartAsync(int userId)
        {
            await _redisDb.KeyDeleteAsync($"cart:{userId}");
        }

        public async Task<IEnumerable<Guid>> GetAllTicketsAsync(int userId)
        {
            var tickets = await _redisDb.SetMembersAsync($"cart:{userId}");
            return Array.ConvertAll(tickets, t => Guid.Parse((string)t));
        }

        public async Task<long> GetCartLengthAsync(int userId)
        {
            return await _redisDb.SetLengthAsync($"cart:{userId}");
        }
    }
}
