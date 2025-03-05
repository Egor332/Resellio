using ResellioBackend.UserManagementSystem.DTOs;
using ResellioBackend.UserManagementSystem.Models;

namespace ResellioBackend.UserManagementSystem.Services.Implementations;

public class TicketTypeService
{
    private readonly ResellioDbContext _context;

    public TicketTypeService(ResellioDbContext context)
    {
        _context = context;
    }

    public async Task<TicketType> CreateTicketTypeAsync(TicketTypeDTO ticketTypeDto, Event createdEvent)
    {
        throw new NotImplementedException();
    }
}