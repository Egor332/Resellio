using ResellioBackend.Common.Pagination;
using ResellioBackend.EventManagementSystem.Filtering;

namespace ResellioBackend.EventManagementSystem.RequestsParameters
{
    public class GetTicketTypesOfEventRequestParameters
    {
        public TicketTypeFilter Filter { get; set; } = new TicketTypeFilter();
        public PaginationRequest Pagination { get; set; } = new PaginationRequest();
    }
}
