using ResellioBackend.TicketPurchaseSystem.Services.Abstractions;
using ResellioBackend.TicketPurchaseSystem.Statics;
using Stripe;

namespace ResellioBackend.TicketPurchaseSystem.Services.Implementations
{
    public class StripeRefundService : IRefundService
    {
        public async Task RefundPaymentAsync(string paymentIntentId, Exception ex)
        {
            var refundService = new Stripe.RefundService();
            await refundService.CreateAsync(new RefundCreateOptions
            {
                PaymentIntent = paymentIntentId,
                Reason = "requested_by_customer",
                Metadata = new Dictionary<string, string>
                {
                    { "reason", ex.Message },
                }
            });
        }
    }
}
