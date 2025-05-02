using ResellioBackend.TicketPurchaseSystem.DTOs;

namespace ResellioBackend.TicketPurchaseSystem.Services.Abstractions
{
    public interface IShoppingCartService
    {
        public Task<CartInfoDto> GetShoppingCartInfoAsync(int userId);
    }
}
