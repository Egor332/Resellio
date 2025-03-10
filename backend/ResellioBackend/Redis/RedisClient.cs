using ResellioBackend.EventManagementSystem.Models.Base;
using StackExchange.Redis;

namespace ResellioBackend.Redis
{
    public class RedisClient : IRedisClient
    {
        private readonly IDatabase _redisDb;

        public RedisClient(IConnectionMultiplexer connectionMultiplexer) 
        {
            _redisDb = connectionMultiplexer.GetDatabase();
        }

    }
}
