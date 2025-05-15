using ResellioBackend.EventManagementSystem.Models.Base;

namespace ResellioBackend.EventManagementSystem.Repositories.Abstractions
{

    public interface ITicketsRepository
    {
        public Task AddAsync(Ticket ticket);

        public Task<Ticket?> GetTicketByIdAsync(Guid ticketId);

        public Task<Ticket?> GetTicketByIdWithExclusiveRowLockAsync(Guid ticketId);

        public Task UpdateAsync(Ticket ticket);

        public Task<Ticket?> GetTicketWithAllDependenciesByIdAsync(Guid ticketId);

        public IQueryable<Ticket> GetMyTicketsAsQueryableNoTracking(int userId);

        public IQueryable<Ticket> GetTicketsOfTypeNoTracking(int ticketTypeId);
    }
}