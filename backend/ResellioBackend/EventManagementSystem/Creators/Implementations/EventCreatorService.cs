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

    public EventCreatorService(IUsersRepository<Organiser> userRepository, IEventsRepository eventRepository)
    {
        _userRepository = userRepository;
        _eventRepository = eventRepository;
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
            End = eventDto.End
        };
        
        await _eventRepository.AddAsync(newEvent);
        
        return new ResultBase()
        {
            Success = true,
            Message = "Created successfully"
        };
    }
}