using ResellioBackend.EventManagementSystem.DTOs;
using ResellioBackend.EventManagementSystem.Models;

namespace ResellioBackend.EventManagementSystem.Creators.Abstractions;

public interface ITicketTypeCreatorService
{
    Task<TicketType> CreateTicketTypeAsync(TicketTypeDto ticketTypeDto, Event createdEvent);
}