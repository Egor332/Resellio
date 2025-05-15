namespace ResellioBackend.EventManagementSystem.RequestsParameters
{
    public class GetTicketOfSelectedTypeParameters :GetTicketsDefaultParameters
    {
        public int TicketTypeId { get; set; }
    }
}
