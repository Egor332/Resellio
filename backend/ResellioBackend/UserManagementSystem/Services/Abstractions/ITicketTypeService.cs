using ResellioBackend.UserManagementSystem.DTOs;
using ResellioBackend.UserManagementSystem.Models;

namespace ResellioBackend.UserManagementSystem.Services.Abstractions;

public interface ITicketTypeService
{
    Task<TicketType> CreateTicketTypeAsync(TicketTypeDTO ticketTypeDto, Event createdEvent);
}