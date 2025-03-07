namespace ResellioBackend.EventManagementSystem.DTOs;

public class EventDto
{
    public int EventId { get; set; }
    
    // no organiser id here – we will get it from the JWT token
    
    public string Name { get; set; }

    public string Description { get; set; }

    public DateTime Start { get; set; }

    public DateTime End { get; set; }
    
    public List<TicketTypeDto> TicketTypes { get; set; }
}