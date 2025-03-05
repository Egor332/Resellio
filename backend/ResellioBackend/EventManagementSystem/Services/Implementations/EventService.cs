using ResellioBackend.EventManagementSystem.DTOs;
using ResellioBackend.EventManagementSystem.Services.Abstractions;
using ResellioBackend.UserManagementSystem.Models;

namespace ResellioBackend.EventManagementSystem.Services.Implementations;

public class EventService: IEventService
{
    private readonly ResellioDbContext _context;

    public EventService(ResellioDbContext context)
    {
        _context = context;
    }

    public async Task<Event> CreateEventAsync(EventDTO eventDto, int organiserId)
    {
        
        var organiser = await _context.Users.FindAsync(organiserId);
        if (organiser == null)
            throw new ArgumentException("Organiser not found");
        
        throw new NotImplementedException();
    }
}