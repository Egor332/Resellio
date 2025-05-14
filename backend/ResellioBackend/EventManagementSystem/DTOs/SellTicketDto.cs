namespace ResellioBackend.EventManagementSystem.DTOs
{
    public class SellTicketDto
    {
        public Guid TicketId { get; set; }
        public decimal Price { get; set; }
        public string Currency { get; set; }
    }
}
