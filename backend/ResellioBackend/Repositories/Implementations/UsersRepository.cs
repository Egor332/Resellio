using Microsoft.EntityFrameworkCore;
using ResellioBackend.Models.Base;
using ResellioBackend.Repositories.Abstractions;

namespace ResellioBackend.Repositories.Implementations
{
    public class UsersRepository<T> : IUsersRepository<T> where T : UserBase
    {
        private readonly ResellioDbContext _context;
        private readonly DbSet<T> _dbSet;

        public UsersRepository(ResellioDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            var entity = await _dbSet.FirstOrDefaultAsync(e => e.UserId == id);
            return entity;
        }

        public async Task<T?> GetByLoginAsync(string login)
        {
            var entity = await _dbSet.FirstOrDefaultAsync(e => e.Login == login);
            return entity;
        }

        public async Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
