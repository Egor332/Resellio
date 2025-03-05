using ResellioBackend.EventManagementSystem.DTOs;
using ResellioBackend.EventManagementSystem.Services.Abstractions;
using ResellioBackend.Results;
using ResellioBackend.UserManagementSystem.Models;
using ResellioBackend.UserManagementSystem.Models.Base;
using ResellioBackend.UserManagementSystem.Models.Users;
using ResellioBackend.UserManagementSystem.Repositories.Abstractions;
using ResellioBackend.UserManagementSystem.Repositories.Implementations;
using ResellioBackend.EventManagementSystem.DTOs;
using ResellioBackend.EventManagementSystem.Services.Abstractions;
using ResellioBackend.EventManagementSystem.Models;

namespace ResellioBackend.EventManagementSystem.Services.Implementations;

public class EventService: IEventService
{
    private readonly ResellioDbContext _context;

    public EventService(ResellioDbContext context)
    {
        _context = context;
    }

    public async Task<Event> CreateEventAsync(EventDto eventDto, int organiserId)
    {
        
        var organiser = await _context.Users.FindAsync(organiserId);
        if (organiser == null)
            throw new ArgumentException("Organiser not found");
        
        var eventEntity = new Event
        {
            Organiser = (Organiser)organiser,
            Name = eventDto.Name,
            Description = eventDto.Description,
            Start = eventDto.Start,
            End = eventDto.End
        };
        
        _context.Events.Add(eventEntity);
        await _context.SaveChangesAsync();
        
        return eventEntity;
    }
}