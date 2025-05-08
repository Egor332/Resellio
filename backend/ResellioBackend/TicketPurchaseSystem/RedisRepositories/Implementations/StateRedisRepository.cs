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
            await _redisDb.SetAddAsync(state, userId.ToString());
        }

        public async Task RemoveStateAsync(string state)
        {
            await _redisDb.KeyDeleteAsync(state);
        }

        public async Task<IEnumerable<int>> GetUserIdAsync(string state)
        {
            var userId = await _redisDb.SetMembersAsync(state);
            return Array.ConvertAll(userId, id => int.Parse((string)id));
        }
    }
}
