using Microsoft.EntityFrameworkCore;
using ResellioBackend.EventManagementSystem.Models.Base;
using ResellioBackend.EventManagementSystem.Repositories.Abstractions;

namespace ResellioBackend.EventManagementSystem.Repositories.Implementations;

public class TicketRepository: ITicketsRepository
{
    private readonly ResellioDbContext _context;
    private readonly DbSet<Ticket> _dbSet;

    public TicketRepository(ResellioDbContext context)
    {
        _context = context;
        _dbSet = context.Set<Ticket>();
    }
    
    public async Task AddAsync(Ticket ticket)
    {
        await _dbSet.AddAsync(ticket);
        await _context.SaveChangesAsync();
    }
}