using ResellioBackend.EventManagementSystem.Models.Base;
using ResellioBackend.TicketPurchaseSystem.RedisRepositories.Abstractions;
using StackExchange.Redis;

namespace ResellioBackend.TicketPurchaseSystem.RedisRepositories.Implementations
{
    public class StateRedisRepository : IStateCacheRepository
    {
        private readonly IDatabase _redisDb;

        public StateRedisRepository(IConnectionMultiplexer connectionMultiplexer)
        {
            _redisDb = connectionMultiplexer.GetDatabase();
        }

        public async Task AddStateAsync(string state, int userId)
        {
            await _redisDb.StringSetAsync(state, userId.ToString(), TimeSpan.FromMinutes(10));
        }

        public async Task RemoveStateAsync(string state)
        {
            await _redisDb.KeyDeleteAsync(state);
        }

        public async Task<string?> GetUserIdAsync(string state)
        {
            return await _redisDb.StringGetAsync(state);
                   
        }
    }
}
