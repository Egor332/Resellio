namespace ResellioBackend.TicketPurchaseSystem.RedisRepositories.Abstractions
{
    public interface ITicketCacheRepository
    {
        public Task<bool> LockTicketAsync(Guid id, TimeSpan timeSpan, int userId);
        public Task<bool> SetExpirationTimeAsync(Guid id, TimeSpan timeSpan);
        public Task UnlockTicketAsync(Guid id);
        public Task<TimeSpan?> GetExpirationTimeAsync(Guid id);
        public Task<int?> GetUserIdAsync(Guid id);
    }
}
