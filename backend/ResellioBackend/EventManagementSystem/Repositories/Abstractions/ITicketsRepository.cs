using ResellioBackend.EventManagementSystem.Models.Base;

namespace ResellioBackend.EventManagementSystem.Repositories.Abstractions;

public interface ITicketsRepository
{
    public Task AddAsync(Ticket ticket);

    public Task<Ticket?> GetTicketByIdAsync(Guid ticketId);

    public Task<Ticket?> GetTicketByIdWithExclusiveRowLockAsync(Guid ticketId);

    public Task UpdateAsync(Ticket ticket);
}