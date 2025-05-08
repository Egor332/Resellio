using ResellioBackend.TicketPurchaseSystem.RedisRepositories.Abstractions;
using ResellioBackend.TicketPurchaseSystem.Services.Abstractions;

namespace ResellioBackend.TicketPurchaseSystem.Services.Implementations
{
    public class SellerRegistrationStateService : ISellerRegistrationStateService
    {
        private readonly IStateCacheRepository _stateCacheRepository;

        public SellerRegistrationStateService(IStateCacheRepository stateCacheRepository)
        {
            _stateCacheRepository = stateCacheRepository;
        }

        public async Task<string> CreateAndStoreStateAsync(int userId)
        {
            string state = Guid.NewGuid().ToString();
            await _stateCacheRepository.AddStateAsync(state, userId);
            return state;
        }

        public async Task RemoveStateAsync(string state)
        {
            await _stateCacheRepository.RemoveStateAsync(state);
        }

        public async Task<int?> ValidateStateAsync(string state)
        {
            var userIds = await _stateCacheRepository.GetUserIdAsync(state);
            if (userIds.Count() != 1)
            {
                return null;
            }
            var userId = userIds.First();
            return userId;
        }
    }
}
