using EllipticCurve.Utils;
using ResellioBackend.EventManagementSystem.Creators.Abstractions;
using ResellioBackend.EventManagementSystem.DTOs;
using ResellioBackend.EventManagementSystem.Models;
using ResellioBackend.EventManagementSystem.ObjectStorages.Abstractions;
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
        private readonly IImageStorage _imageStorage;

        public EventCreatorService(IUsersRepository<Organiser> userRepository, IEventsRepository eventRepository, 
            ITicketTypeCreatorService ticketTypeCreatorService, IImageStorage imageStorage)
        {
            _userRepository = userRepository;
            _eventRepository = eventRepository;
            _ticketTypeCreatorService = ticketTypeCreatorService;
            _imageStorage = imageStorage;
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

            var imageUrl = await StoreImageAndGetImageOrDefaultUrlAsync(eventDto.EventImage);

            try
            {
                var newEvent = new Event
                {
                    Organiser = organiser,
                    Name = eventDto.Name,
                    Description = eventDto.Description,
                    Start = eventDto.Start,
                    End = eventDto.End,
                    TicketTypes = new List<TicketType>(),
                    ImageUrl = imageUrl
                    
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

        private async Task<string> StoreImageAndGetImageOrDefaultUrlAsync(IFormFile image)
        {
            try
            {
                return await _imageStorage.UploadImageAsync(image);
            }
            catch (Exception ex) 
            {
                return "https://upload.wikimedia.org/wikipedia/commons/8/83/TrumpPortrait.jpg";
            }
        }
    }
}