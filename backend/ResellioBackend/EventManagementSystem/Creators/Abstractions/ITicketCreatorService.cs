using ResellioBackend.EventManagementSystem.Models;
using ResellioBackend.EventManagementSystem.Models.Base;
using ResellioBackend.EventManagementSystem.Results;

namespace ResellioBackend.EventManagementSystem.Creators.Abstractions;

public interface ITicketCreatorService
{
    Task<Result<Ticket>> CreateTicketAsync(TicketType ticketType);
}