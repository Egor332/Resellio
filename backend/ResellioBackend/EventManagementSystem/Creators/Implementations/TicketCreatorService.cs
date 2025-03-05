using ResellioBackend.EventManagementSystem.Creators.Abstractions;
using ResellioBackend.EventManagementSystem.Models;
using ResellioBackend.EventManagementSystem.Models.Base;

namespace ResellioBackend.EventManagementSystem.Creators.Implementations;

public class TicketCreatorService: ITicketCreatorService
{
    private readonly ResellioDbContext _context;

    public TicketCreatorService(ResellioDbContext context)
    {
        _context = context;
    }

    public async Task<Ticket> CreateTicketsAsync(TicketType ticketType)
    {
        throw new NotImplementedException();
    }
}