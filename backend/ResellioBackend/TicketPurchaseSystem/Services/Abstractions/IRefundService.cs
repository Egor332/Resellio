namespace ResellioBackend.TicketPurchaseSystem.Services.Abstractions
{
    public interface IRefundService
    {
        public Task RefundPaymentAsync(string paymentIntentId, Exception ex);
    }
}
