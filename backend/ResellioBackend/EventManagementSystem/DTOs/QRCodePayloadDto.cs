namespace ResellioBackend.EventManagementSystem.DTOs
{
    public class QRCodePayloadDto
    {
        public Guid TicketId { get; set; }
        public Guid TemporaryCode { get; set; }
    }
}
