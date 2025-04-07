using ResellioBackend.TicketPurchaseSystem.RedisRepositories.Abstractions;
using StackExchange.Redis;

namespace ResellioBackend.TicketPurchaseSystem.RedisRepositories.Implementations
{
    public class TicketRedisRepository : ITicketRedisRepository
    {
        private readonly IDatabase _redisDb;

        public TicketRedisRepository(IConnectionMultiplexer connectionMultiplexer)
        {
            _redisDb = connectionMultiplexer.GetDatabase();
        }

        public async Task<bool> LockTicketAsync(Guid id, TimeSpan timeSpan, int userId)
        {
            return await _redisDb.StringSetAsync($"ticket:{id}", userId.ToString(), timeSpan, When.NotExists);
        }

        public async Task<bool> SetExpirationTimeAsync(Guid id, TimeSpan timeSpan)
        {
            return await _redisDb.KeyExpireAsync($"ticket:{id}", timeSpan);
        }

        public async Task UnlockTicketAsync(Guid id)
        {
            await _redisDb.KeyDeleteAsync($"ticket:{id}");
        }

        public async Task<TimeSpan?> GetExpirationTimeAsync(Guid id)
        {
            return await _redisDb.KeyTimeToLiveAsync($"ticket:{id}");
        }

        public async Task<int?> GetUserIdAsync(Guid id)
        {
            var userId = await _redisDb.StringGetAsync($"ticket:{id}");
            if (userId.HasValue)
            {
                int parsedUserId;
                if (int.TryParse(userId, out parsedUserId))
                {
                    return parsedUserId;
                }
            }
            return null;
        }
    }
}
