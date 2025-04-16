using ResellioBackend.Results;
using Stripe.Checkout;

namespace ResellioBackend.TicketPurchaseSystem.Results
{
    public class CheckoutSessionCreationResult : ResultBase
    {
        public Session CreatedSession { get; set; }
    }
}
