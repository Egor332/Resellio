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

    public async Task<Result<TicketType>> CreateTicketTypeAsync(TicketTypeDto ticketTypeDto, Event createdEvent)
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

        // TODO: maybe parallelize this
        for (int i = 0; i < ticketTypeDto.MaxCount; i++)
        {
            var result = await _ticketCreatorService.CreateTicketAsync(newTicketType);
            if (result.Success)
                newTicketType.Tickets.Add(result.Data);
            else
                return new Result<TicketType>()
                {
                    Success = false,
                    Message = "Failed to create all tickets"
                    // TODO: right now failure creating one ticket fails the whole event
                };
        }

        await _ticketTypesRepository.AddAsync(newTicketType);

        return new Result<TicketType>()
        {
            Success = true,
            Message = "Created successfully",
            Data = newTicketType
        };

    }
}