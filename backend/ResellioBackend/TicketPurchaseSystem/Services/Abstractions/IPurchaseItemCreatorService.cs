using ResellioBackend.Results;
using ResellioBackend.TicketPurchaseSystem.Results;

namespace ResellioBackend.TicketPurchaseSystem.Services.Abstractions
{
    public interface IPurchaseItemCreatorService
    {
        public Task<PurchaseItemCreationResult> CreatePurchaseItemListAsync(int userId);
    }
}
