using ResellioBackend.Common.Pagination;
using ResellioBackend.EventManagementSystem.Filtering;

namespace ResellioBackend.EventManagementSystem.RequestsParameters
{
    public class GetEventRequestParameters
    {
        public EventsFilter Filter { get; set; } = new EventsFilter();
        public PaginationRequest Pagination { get; set; } = new PaginationRequest();
    }
}
