using ResellioBackend.EventManagementSystem.Models;

namespace ResellioBackend.EventManagementSystem.Repositories.Abstractions;

public interface ITicketTypesRepository
{
    public Task AddAsync(TicketType ticketType);
}