using Microsoft.EntityFrameworkCore;
using ResellioBackend.EventManagementSystem.Models;

namespace ResellioBackend.EventManagementSystem.Repositories.Implementations;

public class EventsRepository
{
    private readonly ResellioDbContext _context;
    private readonly DbSet<Event> _dbSet;
    
    public EventsRepository(ResellioDbContext context)
    {
        _context = context;
        _dbSet = context.Set<Event>();
    }
    
    public async Task AddAsync(Event newEvent)
    {
        await _dbSet.AddAsync(newEvent);
        await _context.SaveChangesAsync();
    }
    
    public async Task UpdateAsync(Event eventToUpdate)
    {
        _dbSet.Update(eventToUpdate);
        await _context.SaveChangesAsync();
    }
}