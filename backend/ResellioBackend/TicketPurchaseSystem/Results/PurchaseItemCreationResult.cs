using ResellioBackend.Results;
using Stripe.Checkout;

namespace ResellioBackend.TicketPurchaseSystem.Results
{
    public class PurchaseItemCreationResult : ResultBase
    {
        public List<SessionLineItemOptions> ItemList { get; set; } = new List<SessionLineItemOptions>();
        public string? SellerAccountId { get; set; }
    }
}
