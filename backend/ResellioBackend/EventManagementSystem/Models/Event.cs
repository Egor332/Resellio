using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using ResellioBackend.UserManagementSystem.Models.Base;
using ResellioBackend.UserManagementSystem.Models.Users;

namespace ResellioBackend.EventManagementSystem.Models;

public class Event
{
    // TODO: add MinimumAge, Location, EventCategory and EventStatus
    
    [Key] 
    public int EventId { get; set; }
    
    [Required] 
    public Organiser Organiser { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string Name { get; set; }
    
    [MaxLength(5000)]
    public string Description { get; set; }
    
    [Required]
    public DateTime Start { get; set; }
    
    [Required]
    public DateTime End { get; set; }
    
    /// <summary>
    /// Name can be a little deceiving. It's a list of TicketTypes not Tickets. Each TicketType remebers all the Tickets of that type.
    /// </summary>
    [Required]
    public List<TicketType> Tickets { get; set; }
}