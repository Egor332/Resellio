namespace ResellioBackend.EventManagementSystem.DTOs;

public class TicketDTO
{
    public int TicketId { get; set; }
    
    public int TicketTypeId { get; set; }

    public int? OwnerId { get; set; }
}