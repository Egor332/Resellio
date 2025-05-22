using ResellioBackend.Common.Filters;
using ResellioBackend.EventManagementSystem.Enums;
using ResellioBackend.EventManagementSystem.Models.Base;

namespace ResellioBackend.EventManagementSystem.Filters
{
    public class TicketsFilter : IFiltrable<Ticket>
    {
        public DateTime? StartsAfter { get; set; }
        public DateTime? EndsBefore { get; set; }
        public TicketSellerFilter? SellerConstraints { get; set; }
        public string? EventNamePart { get; set; }
        

        public IQueryable<Ticket> ApplyFilters(IQueryable<Ticket> query)
        {
            if (StartsAfter != null)
            {
                query = query.Where(t => t.TicketType.Event.Start >= StartsAfter);
            }

            if (EndsBefore != null)
            {
                query = query.Where(t => t.TicketType.Event.End <= EndsBefore);
            }

            if ((SellerConstraints != null) && (SellerConstraints == TicketSellerFilter.OnlyFromOrganiser))
            {
                query = query.Where(t => t.TicketType.Event.OrganiserId == t.HolderId);
            }
            else if ((SellerConstraints != null) && (SellerConstraints == TicketSellerFilter.OnlyNotFromOrganiser))
            {
                query = query.Where(t => t.TicketType.Event.OrganiserId != t.HolderId);
            }


            if (!string.IsNullOrEmpty(EventNamePart))
            {
                query = query.Where(t => t.TicketType.Event.Name.Contains(EventNamePart));
            }

            return query;
        }
    }
}
