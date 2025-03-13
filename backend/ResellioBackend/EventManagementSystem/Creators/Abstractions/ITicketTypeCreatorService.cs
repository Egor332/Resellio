using ResellioBackend.EventManagementSystem.DTOs;
using ResellioBackend.EventManagementSystem.Models;
using ResellioBackend.Results;

namespace ResellioBackend.EventManagementSystem.Creators.Abstractions;

public interface ITicketTypeCreatorService
{
    Task<GeneralResult<TicketType>> CreateTicketTypeAsync(TicketTypeDto ticketTypeDto, Event createdEvent);
}