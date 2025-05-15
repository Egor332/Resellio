using ResellioBackend.Common.Pagination;
using ResellioBackend.EventManagementSystem.Filtering;
using ResellioBackend.EventManagementSystem.Filters;

namespace ResellioBackend.EventManagementSystem.RequestsParameters
{   
    public class GetTicketsDefaultParameters
    {
        public TicketsFilter Filter { get; set; } = new TicketsFilter();
        public PaginationRequest Pagination { get; set; } = new PaginationRequest();
    }
}
