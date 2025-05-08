namespace ResellioBackend.TicketPurchaseSystem.RedisRepositories.Abstractions
{
    public interface IStateCacheRepository
    {
        public Task AddStateAsync(string state, int userId);

        public Task RemoveStateAsync(string state);

        public Task<IEnumerable<int>> GetUserIdAsync(string state);
    }
}
