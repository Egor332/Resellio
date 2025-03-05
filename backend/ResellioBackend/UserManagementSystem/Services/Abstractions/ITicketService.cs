using ResellioBackend.UserManagementSystem.Models;
using ResellioBackend.UserManagementSystem.Models.Base;

namespace ResellioBackend.UserManagementSystem.Services.Abstractions;

public interface ITicketService
{
    Task<Ticket> CreateTicketsAsync(TicketType ticketType);
}