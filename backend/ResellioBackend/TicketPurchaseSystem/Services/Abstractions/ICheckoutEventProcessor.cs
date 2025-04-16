using ResellioBackend.Results;

namespace ResellioBackend.TicketPurchaseSystem.Services.Abstractions
{
    public interface ICheckoutEventProcessor
    {
        public Task<ResultBase> ProcessCheckoutEventAsync(Stripe.Event stripeEvent);
    }
}
