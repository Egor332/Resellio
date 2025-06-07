using ResellioBackend.EventManagementSystem.Repositories.Abstractions;
using StackExchange.Redis;
using System;

namespace ResellioBackend.EventManagementSystem.Repositories.Implementations
{
    public class QRCodeTemporaryCodeRedisRepository : IQRCodeTemporaryCodeRepository
    {
        private readonly IDatabase _redisDb;

        public QRCodeTemporaryCodeRedisRepository(IConnectionMultiplexer connectionMultiplexer)
        {
            _redisDb = connectionMultiplexer.GetDatabase();
        }

        private readonly TimeSpan _defaultExpirationTime = TimeSpan.FromMinutes(10);
        public async Task<bool> AddTemporaryCodeAsync(Guid code)
        {
            return await _redisDb.StringSetAsync($"code:{code}", "", _defaultExpirationTime, When.NotExists);            
        }

        public async Task<bool> IsTemporaryCodeExistAsync(Guid code)
        {
            return await _redisDb.KeyExistsAsync($"code:{code}");
        }

        public async Task RemoveTemporaryCodeAsync(Guid code)
        {
            await _redisDb.KeyDeleteAsync($"code:{code}");
        }
    }
}
