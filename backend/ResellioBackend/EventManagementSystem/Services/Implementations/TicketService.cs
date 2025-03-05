using ResellioBackend.UserManagementSystem.Models;

namespace ResellioBackend.EventManagementSystem.Services.Implementations;

public class TicketService
{
    private readonly ResellioDbContext _context;

    public TicketService(ResellioDbContext context)
    {
        _context = context;
    }

    public async Task CreateTicketsAsync(TicketType ticketType)
    {
        throw new NotImplementedException();
    }
}