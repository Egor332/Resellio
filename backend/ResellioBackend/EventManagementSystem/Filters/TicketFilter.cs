using ResellioBackend.Common.Filters;
using ResellioBackend.EventManagementSystem.Models.Base;

namespace ResellioBackend.EventManagementSystem.Filters
{
    public class TicketFilter : IFiltrable<Ticket>
    {
        public int TicketTypeId { get; set; }

        public IQueryable<Ticket> ApplyFilters(IQueryable<Ticket> query)
        {
            query.Where(t => t.TicketTypeId == TicketTypeId);

            return query;
        }
    }
}
