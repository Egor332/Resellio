using ResellioBackend.EventManagementSystem.Creators.Abstractions;
using ResellioBackend.EventManagementSystem.DTOs;
using ResellioBackend.EventManagementSystem.Models;
using ResellioBackend.EventManagementSystem.Models.Base;
using ResellioBackend.EventManagementSystem.Repositories.Abstractions;
using ResellioBackend.Results;

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

    public async Task<GeneralResult<TicketType>> CreateTicketTypeAsync(TicketTypeDto ticketTypeDto, Event createdEvent)
    {
        TicketType newTicketType = new TicketType()
        {
            Event = createdEvent,
            Description = ticketTypeDto.Description,
            MaxCount = ticketTypeDto.MaxCount,
            Price = ticketTypeDto.Price,
            Currency = ticketTypeDto.Currency,
            AvailableFrom = ticketTypeDto.AvailableFrom,
            Tickets = new List<Ticket>()
        };

        for (int i = 0; i < ticketTypeDto.MaxCount; i++)
        {
            var result = await _ticketCreatorService.CreateTicketAsync(newTicketType);
            if (result.Success)
                newTicketType.Tickets.Add(result.Data);
            else
                return new GeneralResult<TicketType>()
                {
                    Success = false,
                    Message = result.Message
                    // TODO: right now failure creating one ticket fails the whole event
                };
        }

        return new GeneralResult<TicketType>()
        {
            Success = true,
            Message = "Created successfully",
            Data = newTicketType
        };

    }
}