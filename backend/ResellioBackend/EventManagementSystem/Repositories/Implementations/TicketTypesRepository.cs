using Microsoft.EntityFrameworkCore;
using ResellioBackend.EventManagementSystem.Models;
using ResellioBackend.EventManagementSystem.Repositories.Abstractions;

namespace ResellioBackend.EventManagementSystem.Repositories.Implementations
{

    public class TicketTypesRepository : ITicketTypesRepository
    {
        private readonly ResellioDbContext _context;
        private readonly DbSet<TicketType> _dbSet;

        public TicketTypesRepository(ResellioDbContext context)
        {
            _context = context;
            _dbSet = context.Set<TicketType>();
        }

        public async Task AddAsync(TicketType ticketType)
        {
            await _dbSet.AddAsync(ticketType);
            await _context.SaveChangesAsync();
        }
    }
}