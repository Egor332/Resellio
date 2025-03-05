using ResellioBackend.EventManagementSystem.DTOs;
using ResellioBackend.EventManagementSystem.Models;

namespace ResellioBackend.EventManagementSystem.Services.Abstractions;

public interface ITicketTypeService
{
    Task<TicketType> CreateTicketTypeAsync(TicketTypeDto ticketTypeDto, Event createdEvent);
}