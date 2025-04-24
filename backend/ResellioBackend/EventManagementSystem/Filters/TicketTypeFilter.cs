using ResellioBackend.Common.Filters;
using ResellioBackend.EventManagementSystem.Models;

namespace ResellioBackend.EventManagementSystem.Filtering
{
    public class TicketTypeFilter : IFiltrable<TicketType>
    {
        public int EventId { get; set; }


        public IQueryable<TicketType> ApplyFilters(IQueryable<TicketType> query)
        {
            query.Where(tt  => tt.EventId == EventId);
            return query;
        }
    }
}
