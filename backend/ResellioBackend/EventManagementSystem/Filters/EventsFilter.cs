using ResellioBackend.Common.Filters;
using ResellioBackend.EventManagementSystem.Models;

namespace ResellioBackend.EventManagementSystem.Filtering
{
    public class EventsFilter : IFiltrable<Event>
    {
        public List<int>? OrganiserIds { get; set; }

        public IQueryable<Event> ApplyFilters(IQueryable<Event> query)
        {
            if ((OrganiserIds != null) && (OrganiserIds.Count > 0))
            {
                query.Where(e => OrganiserIds.Contains(e.OrganiserId));
            }
            return query;
        }
    }
}
