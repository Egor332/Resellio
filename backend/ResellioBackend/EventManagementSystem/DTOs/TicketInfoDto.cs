using ResellioBackend.EventManagementSystem.Models;

namespace ResellioBackend.EventManagementSystem.DTOs
{
    public class TicketInfoDto
    {
        public Guid Id { get; set; }
        public bool IsOnSale { get; set; }
        public Money CurrentPrice { get; set; }
        public string EventName { get; set; }
        public string EventDescription { get; set; }
        public string TicketTypeDescription { get; set; }
        public bool IsHoldByOrganiser { get; set; }
        public int EventId { get; set; }
        public int TicketTypeId { get; set; }
    }
}
