using Stripe.Checkout;

namespace ResellioBackend.TicketPurchaseSystem.Services.Abstractions
{
    public interface ICheckoutSessionManagerService
    {
        public int? GetUserIdOrNullFromSessionMetadata(Session session);
        public Task<List<Guid>?> GetTicketIdsOrNullFromSessionAsync(Session session);
    }
}
