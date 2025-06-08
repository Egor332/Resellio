using ResellioBackend.TicketPurchaseSystem.Services.Abstractions;
using ResellioBackend.UserManagementSystem.Models.Base;
using ResellioBackend.UserManagementSystem.Repositories.Abstractions;

namespace ResellioBackend.TicketPurchaseSystem.Services.Implementations
{
    public class SellerRegistrationService : ISellerRegistrationService 
    {
        private readonly ISellerRegistrationStateService _stateService;
        private readonly IUsersRepository<UserBase> _usersRepository;
        private readonly string _clientId;

        public SellerRegistrationService(ISellerRegistrationStateService stateService, IUsersRepository<UserBase> usersRepository, IConfiguration configuration)
        {
            _stateService = stateService;
            _usersRepository = usersRepository;
            _clientId = configuration["Stripe:ClientId"];
        }

        public async Task<string> StartRegistrationAsync(int userId, string redirectUri)
        {
            var state = await _stateService.CreateAndStoreStateAsync(userId);

            var stripeUrl = $"https://connect.stripe.com/oauth/authorize" +
                    $"?response_type=code" +
                    $"&client_id={_clientId}" +
                    $"&scope=read_write" +
                    $"&state={state}";

            return stripeUrl;
        }

        public async Task<bool> CompleteRegistrationAsync(string code, string state)
        {
            var userId = await _stateService.ValidateStateAsync(state);
            if (userId == null)
            {
                return false;
            }

            var service = new Stripe.OAuthTokenService();

            var options = new Stripe.OAuthTokenCreateOptions
            {
                GrantType = "authorization_code",
                Code = code,
            };

            var response = await service.CreateAsync(options);
            var connectedAccountId = response.StripeUserId;            

            var user = await _usersRepository.GetByIdAsync(userId.Value);
            if (user == null)
            {
                return false;
            }
            user.ConnectedSellingAccount = connectedAccountId;
            await _usersRepository.UpdateAsync(user);

            return true;
        }        
    }
}
