using ResellioBackend.EventManagementSystem.DTOs;
using ResellioBackend.UserManagementSystem.Models;

namespace ResellioBackend.EventManagementSystem.Services.Abstractions;

public interface ITicketTypeService
{
    Task<TicketType> CreateTicketTypeAsync(TicketTypeDTO ticketTypeDto, Event createdEvent);
}