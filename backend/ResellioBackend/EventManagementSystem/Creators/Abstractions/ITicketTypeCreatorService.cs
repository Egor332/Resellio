using ResellioBackend.EventManagementSystem.DTOs;
using ResellioBackend.EventManagementSystem.Models;
using ResellioBackend.Results;

namespace ResellioBackend.EventManagementSystem.Creators.Abstractions;

public interface ITicketTypeCreatorService
{
    Task<Result<TicketType>> CreateTicketTypeAsync(TicketTypeDto ticketTypeDto, Event createdEvent);
}