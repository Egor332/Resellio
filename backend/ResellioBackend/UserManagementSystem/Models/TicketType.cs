namespace ResellioBackend.UserManagementSystem.Models;
using System.ComponentModel.DataAnnotations;

public class TicketType
{
    [Key] 
    public int TicketId { get; set; }
    
    [Required]
    public Event Event { get; set; }
    
    public string Description { get; set; }
    
    [Required]
    public int MaxCount { get; set; }
    
    [Required]
    public decimal Price { get; set; }

    [Required]
    public string Currency { get; set; }
    
    [Required]
    public DateTime AvailableFrom { get; set; }
}