using ResellioBackend.EventManagementSystem.Models;
using ResellioBackend.EventManagementSystem.Models.Base;

namespace ResellioBackend.EventManagementSystem.Creators.Abstractions;

public interface ITicketCreatorService
{
    Task<Ticket> CreateTicketsAsync(TicketType ticketType);
}