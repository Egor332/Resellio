using ResellioBackend.EventManagementSystem.Models;

namespace ResellioBackend.EventManagementSystem.DTOs
{
    public class TicketTypeInfoDto
    {
        public int TypeId { get; set; }
        public string Description { get; set; }
        public DateTime AvailableFrom { get; set; }
        public int AmountOfTickets { get; set; } // Not available tickets, but in general
        public Money BasePrice { get; set; }
    }
}
