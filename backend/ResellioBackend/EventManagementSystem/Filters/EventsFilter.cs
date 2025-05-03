using ResellioBackend.Common.Filters;
using ResellioBackend.EventManagementSystem.Models;

namespace ResellioBackend.EventManagementSystem.Filtering
{
    public class EventsFilter : IFiltrable<Event>
    {
        public List<int>? EventIds { get; set; }
        public List<int>? OrganiserIds { get; set; }
        public string? OrganiserNamePart { get; set; }
        public DateTime? StartsAfter { get; set; }
        public DateTime? EndsBefore { get; set; }
        public string? NamePart { get; set; }

        public IQueryable<Event> ApplyFilters(IQueryable<Event> query)
        {
            if ((EventIds != null) && (EventIds.Count > 0))
            {
                query = query.Where(e => EventIds.Contains(e.EventId));
            }

            if ((OrganiserIds != null) && (OrganiserIds.Count > 0))
            {
                query = query.Where(e => OrganiserIds.Contains(e.OrganiserId));
            }

            if (!string.IsNullOrEmpty(OrganiserNamePart))
            {
                query = query.Where(e => e.Organiser.OrganiserName.Contains(OrganiserNamePart));
            }

            if (StartsAfter != null)
            {
                query = query.Where(e => e.Start >= StartsAfter);
            }

            if (EndsBefore != null)
            {
                query = query.Where(e => e.End <= EndsBefore);
            }

            if (!string.IsNullOrEmpty(NamePart))
            {
                query = query.Where(e => e.Name.Contains(NamePart));
            }
            return query;
        }
    }
}
