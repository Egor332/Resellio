using ResellioBackend.Common.Paging;
using ResellioBackend.EventManagementSystem.DTOs;
using ResellioBackend.EventManagementSystem.Filtering;

namespace ResellioBackend.EventManagementSystem.Services.Abstractions
{
    public interface IEventService
    {
        public Task<PaginationResult<EventInfoDto>> GetFiltratedEventsWithPagingAsync(EventsFilter filter, int page = 1, int pageSize = 10);
    }
}
