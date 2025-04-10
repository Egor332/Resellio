using Microsoft.EntityFrameworkCore;
using ResellioBackend.EventManagementSystem.Models.Base;
using ResellioBackend.EventManagementSystem.Repositories.Abstractions;

namespace ResellioBackend.EventManagementSystem.Repositories.Implementations
{

    public class TicketsRepository : ITicketsRepository
    {
        private readonly ResellioDbContext _context;
        private readonly DbSet<Ticket> _dbSet;

        public TicketsRepository(ResellioDbContext context)
        {
            _context = context;
            _dbSet = context.Set<Ticket>();
        }

        public async Task AddAsync(Ticket ticket)
        {
            await _dbSet.AddAsync(ticket);
            await _context.SaveChangesAsync();
        }

        public async Task<Ticket?> GetTicketByIdAsync(Guid ticketId)
        {
            return await _dbSet.FirstOrDefaultAsync(t => t.TicketId == ticketId);
        }

        public async Task UpdateAsync(Ticket ticket)
        {
            _dbSet.Update(ticket);
            await _context.SaveChangesAsync();
        }

        public async Task<Ticket?> GetTicketByIdWithExclusiveRowLockAsync(Guid ticketId)
        {
            return await _dbSet.FromSqlRaw("SELECT * FROM Tickets WITH (XLOCK, ROWLOCK) WHERE TicketId = @p0", ticketId)
            .FirstOrDefaultAsync();
        }

        public async Task<Ticket?> GetTicketWithAllDependenciesByIdAsync(Guid ticketId)
        {
            return await _dbSet // TODO: consider NoTracking mode                
                .Include(t => t.Holder)
                .Include(t => t.PurchaseIntender)
                .Include(t => t.TicketType)
                .ThenInclude(tt => tt.Event)
                .FirstOrDefaultAsync(t => t.TicketId == ticketId);
        }
    }
}