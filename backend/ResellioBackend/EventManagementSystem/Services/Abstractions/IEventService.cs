using ResellioBackend.Common.Filters;
using ResellioBackend.Common.Paging;
using ResellioBackend.EventManagementSystem.DTOs;
using ResellioBackend.EventManagementSystem.Models;

namespace ResellioBackend.EventManagementSystem.Services.Abstractions
{
    public interface IEventService
    {
        public Task<PaginationResult<EventInfoDto>> GetEventsAsync(IFiltrable<Event> filter, int page = 1, int pageSize = 10);
    }
}
