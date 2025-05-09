namespace ResellioBackend.TicketPurchaseSystem.RedisRepositories.Abstractions
{
    public interface IStateCacheRepository
    {
        public Task AddStateAsync(string state, int userId);

        public Task RemoveStateAsync(string state);

        public Task<string?> GetUserIdAsync(string state);
    }
}
