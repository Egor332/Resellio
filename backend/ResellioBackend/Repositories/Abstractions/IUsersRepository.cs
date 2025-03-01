using ResellioBackend.Models.Base;

namespace ResellioBackend.Repositories.Abstractions
{
    public interface IUsersRepository<T> where T : UserBase
    {
        public Task<T?> GetByIdAsync(int id);
        public Task<T?> GetByLoginAsync(string login);
        public Task UpdateAsync(T entity);
        public Task AddAsync(T entity);
        public Task DeleteAsync(T entity);

    }
}
