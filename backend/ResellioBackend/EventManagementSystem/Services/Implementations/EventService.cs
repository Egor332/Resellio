using ResellioBackend.Common.Paging;
using ResellioBackend.EventManagementSystem.DTOs;
using ResellioBackend.EventManagementSystem.Filtering;
using ResellioBackend.EventManagementSystem.Models;
using ResellioBackend.EventManagementSystem.Repositories.Abstractions;
using ResellioBackend.EventManagementSystem.Services.Abstractions;

namespace ResellioBackend.EventManagementSystem.Services.Implementations
{
    public class EventService : IEventService
    {
        private readonly IEventsRepository _eventsRepository;
        private readonly IPaginationService _pagingService;

        public EventService(IEventsRepository eventsRepository, IPaginationService pagingService)
        {
            _eventsRepository = eventsRepository;
            _pagingService = pagingService;
        }

        public async Task<PaginationResult<EventInfoDto>> GetFiltratedEventsWithPagingAsync(EventsFilter filter, int page = 1, int pageSize = 10)
        {
            var query = _eventsRepository.GetAllAsQueryable();
            query = filter.ApplyFilters(query);
            var paginatedModels = new PaginationResult<Event>();
            try
            {
                paginatedModels = await _pagingService.ApplyPagingAsync<Event>(query, page, pageSize);
            }
            catch (ArgumentOutOfRangeException)
            {
                throw;
            }

            var paginationResponse = new PaginationResult<EventInfoDto>()
            {
                Items = new List<EventInfoDto>(),
                PageNumber = paginatedModels.PageNumber,
                TotalAmount = paginatedModels.TotalAmount,
            };
            foreach (var item in paginatedModels.Items) 
            { 
                var itemDto = EventToEventInfoDto(item);
                paginationResponse.Items.Add(itemDto);
            }

            return paginationResponse;
        }

        private EventInfoDto EventToEventInfoDto(Event model)
        {
            return new EventInfoDto()
            {
                Id = model.EventId,
                Name = model.Name,
                Description = model.Description,
                Start = model.Start,
                End = model.End,
            };
        }
    }
}
