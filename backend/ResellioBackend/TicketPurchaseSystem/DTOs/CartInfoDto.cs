using ResellioBackend.EventManagementSystem.DTOs;

namespace ResellioBackend.TicketPurchaseSystem.DTOs
{
    public class CartInfoDto
    {
        public bool IsCartExist { get; set; }
        public List<TicketInfoDto>? ticketsInCart { get; set; }
        public DateTime? CartExpirationTime { get; set; }
    }
}
