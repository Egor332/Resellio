using ResellioBackend.UserManagementSystem.Models;
using ResellioBackend.UserManagementSystem.Models.Base;

namespace ResellioBackend.EventManagementSystem.Services.Abstractions;

public interface ITicketService
{
    Task<Ticket> CreateTicketsAsync(TicketType ticketType);
}