using ResellioBackend.TicketPurchaseSystem.Results;
using ResellioBackend.TicketPurchaseSystem.Services.Abstractions;
using Stripe.Checkout;

namespace ResellioBackend.TicketPurchaseSystem.Services.Implementations
{
    public class StripeCheckoutSessionCreatorService : ICheckoutSessionCreatorService
    {
        private readonly IPurchaseItemCreatorService _purchaseItemCreatorService;
        private readonly IPurchaseLockService _purchaseLockService;
        private readonly string _successLink;
        private readonly string _cancelLink;

        private static readonly List<string> _paymentMethods = new List<string>() { "card" };

        public StripeCheckoutSessionCreatorService(IPurchaseItemCreatorService purchaseItemCreatorService, IPurchaseLockService purchaseLockService,
            IConfiguration configuration)
        {
            _purchaseItemCreatorService = purchaseItemCreatorService;
            _purchaseLockService = purchaseLockService;
            _successLink = configuration["FrontEndLinks:PaymentSuccess"]!;
            _cancelLink = configuration["FrontEndLinks:PaymentCancel"]!;
        }

        public async Task<CheckoutSessionCreationResult> CreateCheckoutSession(int userId)
        {
            var lineItemsResult = await _purchaseItemCreatorService.CreatePurchaseItemListAsync(userId);
            if (!lineItemsResult.Success) 
            {
                return new CheckoutSessionCreationResult()
                {
                    Success = false,
                    Message = lineItemsResult.Message,
                };
            }
            var lockExtensionResult = await _purchaseLockService.EnsureEnoughLockTimeForPurchaseAsync(userId);
            if (!lockExtensionResult.Success) 
            {
                return new CheckoutSessionCreationResult()
                {
                    Success = false,
                    Message = lockExtensionResult.Message,
                };
            }
            var options = new SessionCreateOptions()
            {
                PaymentMethodTypes = _paymentMethods,
                LineItems = lineItemsResult.ItemList,
                Mode = "payment",
                SuccessUrl = _successLink,
                CancelUrl = _cancelLink,
                Metadata = new Dictionary<string, string>()
                {
                    { "userId", userId.ToString() }
                }
            };

            var service = new SessionService();
            Session createdSession = await service.CreateAsync(options);
            return new CheckoutSessionCreationResult()
            {
                Success = true,
                CreatedSession = createdSession,
            };
        }
    }
}
