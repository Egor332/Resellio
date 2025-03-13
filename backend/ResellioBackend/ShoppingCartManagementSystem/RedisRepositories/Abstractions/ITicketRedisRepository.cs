namespace ResellioBackend.ShoppingCartManagementSystem.RedisRepositories.Abstractions
{
    public interface ITicketRedisRepository
    {
        public Task<bool> LockTicketAsync(Guid id, TimeSpan timeSpan);
        public Task SetExpirationTimeAsync(Guid id, TimeSpan timeSpan);
        public Task UnlockTicketAsync(Guid id);
        public Task<TimeSpan?> GetExpirationTimeAsync(Guid id);
    }
}
