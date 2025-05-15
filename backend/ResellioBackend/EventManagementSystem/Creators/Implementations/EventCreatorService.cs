using ResellioBackend.EventManagementSystem.Creators.Abstractions;
using ResellioBackend.EventManagementSystem.DTOs;
using ResellioBackend.EventManagementSystem.Models;
using ResellioBackend.EventManagementSystem.Repositories.Abstractions;
using ResellioBackend.Results;
using ResellioBackend.UserManagementSystem.Models.Base;
using ResellioBackend.UserManagementSystem.Models.Users;
using ResellioBackend.UserManagementSystem.Repositories.Abstractions;
using ResellioBackend.UserManagementSystem.Statics;

namespace ResellioBackend.EventManagementSystem.Creators.Implementations
{

    public class EventCreatorService : IEventCreatorService
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

            if (!organiser.ValidateAbilityToSale())
            {
                return new ResultBase()
                {
                    Success = false,
                    Message = "You have not connect selling account", 
                    ErrorCode = UserManagementSystemErrorsCodes.UserDoesNotConnectSellerAccount
                };
            }

            try
            {
                var newEvent = new Event
                {
                    Organiser = organiser,
                    Name = eventDto.Name,
                    Description = eventDto.Description,
                    Start = eventDto.Start,
                    End = eventDto.End,
                    TicketTypes = new List<TicketType>()
                };

                foreach (TicketTypeDto ticketTypeDto in eventDto.TicketTypeDtosList)
                {
                    var result = _ticketTypeCreatorService.CreateTicketType(ticketTypeDto, newEvent);
                    if (result.Success)
                        newEvent.TicketTypes.Add(result.Data); // this will also make EF add the TicketTypes to the database
                    else
                        return new ResultBase()
                        {
                            Success = false,
                            Message = result.Message
                        };
                }

                await _eventRepository.AddAsync(newEvent);

                return new ResultBase()
                {
                    Success = true,
                    Message = "Created successfully"
                };
            }
            catch (Exception ex)
            {
                return new ResultBase()
                {
                    Success = false,
                    Message = $"Error creating event: {ex.Message}"
                };
            }
        }
    }
}