namespace ResellioBackend.EventManagementSystem.DTOs;

public class TicketTypeDto
{
    public string Description { get; set; }
    
    public int MaxCount { get; set; }
    
    public decimal Price { get; set; }
    
    public string Currency { get; set; }
    
    public DateTime AvailableFrom { get; set; }
}