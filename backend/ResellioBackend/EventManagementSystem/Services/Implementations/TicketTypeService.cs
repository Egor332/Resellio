using ResellioBackend.EventManagementSystem.DTOs;
using ResellioBackend.EventManagementSystem.Models;

namespace ResellioBackend.EventManagementSystem.Services.Implementations;

public class TicketTypeService
{
    private readonly ResellioDbContext _context;

    public TicketTypeService(ResellioDbContext context)
    {
        _context = context;
    }

    public async Task<TicketType> CreateTicketTypeAsync(TicketTypeDto ticketTypeDto, Event createdEvent)
    {
        throw new NotImplementedException();
    }
}