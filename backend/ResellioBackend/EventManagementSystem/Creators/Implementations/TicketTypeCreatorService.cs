using ResellioBackend.EventManagementSystem.Creators.Abstractions;
using ResellioBackend.EventManagementSystem.DTOs;
using ResellioBackend.EventManagementSystem.Models;
using ResellioBackend.EventManagementSystem.Models.Base;
using ResellioBackend.EventManagementSystem.Repositories.Abstractions;
using ResellioBackend.EventManagementSystem.Results;

namespace ResellioBackend.EventManagementSystem.Creators.Implementations;

public class TicketTypeCreatorService: ITicketTypeCreatorService
{
    public readonly ITicketTypesRepository _ticketTypesRepository;
    public readonly ITicketCreatorService _ticketCreatorService;

    public TicketTypeCreatorService(ITicketTypesRepository ticketTypesRepository, ITicketCreatorService ticketCreatorService)
    {
        _ticketTypesRepository = ticketTypesRepository;
        _ticketCreatorService = ticketCreatorService;
    }

    public async Task<Result<TicketType>> CreateTicketTypeAsync(TicketTypeDto ticketTypeDto, Event createdEvent)
    {
        throw new NotImplementedException();
    }
}