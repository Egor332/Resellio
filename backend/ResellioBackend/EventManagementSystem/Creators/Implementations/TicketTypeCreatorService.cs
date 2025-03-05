using ResellioBackend.EventManagementSystem.Creators.Abstractions;
using ResellioBackend.EventManagementSystem.DTOs;
using ResellioBackend.EventManagementSystem.Models;

namespace ResellioBackend.EventManagementSystem.Creators.Implementations;

public class TicketTypeCreatorService: ITicketTypeCreatorService
{
    private readonly ResellioDbContext _context;

    public TicketTypeCreatorService(ResellioDbContext context)
    {
        _context = context;
    }

    public async Task<TicketType> CreateTicketTypeAsync(TicketTypeDto ticketTypeDto, Event createdEvent)
    {
        throw new NotImplementedException();
    }
}