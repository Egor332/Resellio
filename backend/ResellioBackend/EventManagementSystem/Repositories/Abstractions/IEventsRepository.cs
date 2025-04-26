using ResellioBackend.EventManagementSystem.Models;

namespace ResellioBackend.EventManagementSystem.Repositories.Abstractions
{
    public interface IEventsRepository
    {
        public Task AddAsync(Event eventToBeAdded);
        public Task UpdateAsync(Event eventToBeUpdated);
        public IQueryable<Event> GetAllAsQueryable();
    }
}