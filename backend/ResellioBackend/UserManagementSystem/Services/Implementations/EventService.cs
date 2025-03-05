using ResellioBackend.UserManagementSystem.DTOs;
using ResellioBackend.UserManagementSystem.Models;
using ResellioBackend.UserManagementSystem.Services.Abstractions;

namespace ResellioBackend.UserManagementSystem.Services.Implementations;

public class EventService: IEventService
{
    private readonly ResellioDbContext _context;

    public EventService(ResellioDbContext context)
    {
        _context = context;
    }

    public async Task<Event> CreateEventAsync(EventDTO eventDto)
    {
        throw new NotImplementedException();
    }
}