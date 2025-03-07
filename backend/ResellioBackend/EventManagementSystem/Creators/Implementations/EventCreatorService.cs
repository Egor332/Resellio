using ResellioBackend.EventManagementSystem.Creators.Abstractions;
using ResellioBackend.EventManagementSystem.DTOs;
using ResellioBackend.EventManagementSystem.Models;
using ResellioBackend.EventManagementSystem.Repositories.Abstractions;
using ResellioBackend.Results;
using ResellioBackend.UserManagementSystem.Models.Base;
using ResellioBackend.UserManagementSystem.Models.Users;
using ResellioBackend.UserManagementSystem.Repositories.Abstractions;

namespace ResellioBackend.EventManagementSystem.Creators.Implementations;

public class EventCreatorService: IEventCreatorService
{
    private readonly IUsersRepository<Organiser> _userRepository;
    private readonly IEventsRepository _eventRepository;
    private readonly ITicketTypeCreatorService _ticketTypeCreatorService;

    public EventCreatorService(IUsersRepository<Organiser> userRepository, IEventsRepository eventRepository, ITicketTypeCreatorService ticketTypeCreatorService)
    {
        _userRepository = userRepository;
        _eventRepository = eventRepository;
        _ticketTypeCreatorService = ticketTypeCreatorService;
    }

    public async Task<ResultBase> CreateEventAsync(EventDto eventDto, int organiserId)
    {
        var organiser = await _userRepository.GetByIdAsync(organiserId);
        if (organiser == null)
        {
            return new ResultBase()
            {
                Success = false,
                Message = "Organiser not found"
            };
        }
        
        var newEvent = new Event
        {
            Organiser = organiser,
            Name = eventDto.Name,
            Description = eventDto.Description,
            Start = eventDto.Start,
            End = eventDto.End,
            TicketTypes = new List<TicketType>()
        };
        
        // Create ticket types in parallel instead of a sequential foreach loop
        // var ticketTypeTasks = eventDto.TicketTypes
        //     .Select(ticketTypeDto => _ticketTypeCreatorService.CreateTicketTypeAsync(ticketTypeDto, newEvent))
        //     .ToList();
        //
        // var ticketTypes = await Task.WhenAll(ticketTypeTasks);
        // newEvent.TicketTypes.AddRange(ticketTypes);
        
        // for now let's keep it sequential as it's easier to understand
        foreach (TicketTypeDto ticketTypeDto in eventDto.TicketTypes)
        {
            newEvent.TicketTypes.Add(await _ticketTypeCreatorService.CreateTicketTypeAsync(ticketTypeDto, newEvent));
        }
        
        await _eventRepository.AddAsync(newEvent);
        
        return new ResultBase()
        {
            Success = true,
            Message = "Created successfully"
        };
    }
}