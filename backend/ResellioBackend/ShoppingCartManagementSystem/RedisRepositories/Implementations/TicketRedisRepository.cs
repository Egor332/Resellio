using ResellioBackend.ShoppingCartManagementSystem.RedisRepositories.Abstractions;
using StackExchange.Redis;

namespace ResellioBackend.ShoppingCartManagementSystem.RedisRepositories.Implementations
{
    public class TicketRedisRepository : ITicketRedisRepository
    {
        private readonly IDatabase _redisDb;

        public TicketRedisRepository(IConnectionMultiplexer connectionMultiplexer)
        {
            _redisDb = connectionMultiplexer.GetDatabase();
        }

        public async Task<bool> LockTicketAsync(Guid id, TimeSpan timeSpan)
        {
            return await _redisDb.StringSetAsync($"ticket:{id}", "", timeSpan, When.NotExists);
        }

        public async Task SetExpirationTimeAsync(Guid id, TimeSpan timeSpan)
        {
            await _redisDb.KeyExpireAsync($"ticket:{id}", timeSpan);
        }

        public async Task UnlockTicketAsync(Guid id)
        {
            await _redisDb.KeyDeleteAsync($"ticket:{id}");
        }
    }
}
