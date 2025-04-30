using ResellioBackend.Common.Filters;
using ResellioBackend.Common.Paging;
using ResellioBackend.EventManagementSystem.DTOs;
using ResellioBackend.EventManagementSystem.Mapper;
using ResellioBackend.EventManagementSystem.Models;
using ResellioBackend.EventManagementSystem.Repositories.Abstractions;
using ResellioBackend.EventManagementSystem.Services.Abstractions;

namespace ResellioBackend.EventManagementSystem.Services.Implementations
{
    public class EventService : IEventService
    {
        private readonly IEventsRepository _eventsRepository;
        private readonly IGetRequestService _getRequestService;

        public EventService(IEventsRepository eventsRepository, IGetRequestService getRequestService) 
        { 
            _eventsRepository = eventsRepository; 
            _getRequestService = getRequestService;
        }

        public async Task<PaginationResult<EventInfoDto>> GetEventsAsync(IFiltrable<Event> filter, int page = 1, int pageSize = 10)
        {
            var query = _eventsRepository.GetAllAsQueryable();
            try
            {
                return await _getRequestService.ApplyFiltersAndPaginationAsync<Event, EventInfoDto>
                    (query, EventManagementSystemMapper.EventToEventInfoDto, filter, page, pageSize);
            }
            catch (ArgumentOutOfRangeException)
            {
                throw;
            }
        } 

    }
}
