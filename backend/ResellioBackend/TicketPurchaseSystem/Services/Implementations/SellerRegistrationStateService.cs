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
            var userIdStr = await _stateCacheRepository.GetUserIdAsync(state);
            if (string.IsNullOrEmpty(userIdStr))
            {
                return null;
            }
            int userId;

            if (int.TryParse(userIdStr, out userId))
            {
                return userId;
            }
            else
            {
                return null;
            }
        }
    }
}
