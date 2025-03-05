using ResellioBackend.EventManagementSystem.Models;
using ResellioBackend.EventManagementSystem.Models.Base;

namespace ResellioBackend.EventManagementSystem.Services.Abstractions;

public interface ITicketService
{
    Task<Ticket> CreateTicketsAsync(TicketType ticketType);
}