using ResellioBackend.TicketPurchaseSystem.Results;

namespace ResellioBackend.TicketPurchaseSystem.Services.Abstractions
{
    public interface ICheckoutSessionCreatorService
    {
        public Task<CheckoutSessionCreationResult> CreateCheckoutSessionAsync(int userId);
    }
}
