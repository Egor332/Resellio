using ResellioBackend.UserManagementSystem.Models.Users;

namespace ResellioBackend.UserManagementSystem.DTOs;

public class EventDTO
{
    public int EventId { get; set; }
    
    public int OrganiserId { get; set; }
    
    public string Name { get; set; }

    public string Description { get; set; }

    public DateTime Start { get; set; }

    public DateTime End { get; set; }
    
    public List<int> TicketTypeIds { get; set; }
}