using ResellioBackend.EventManagementSystem.Models.Base;
using ResellioBackend.EventManagementSystem.Repositories.Abstractions;
using ResellioBackend.Results;
using ResellioBackend.TicketPurchaseSystem.RedisRepositories.Abstractions;
using ResellioBackend.TicketPurchaseSystem.RedisServices.Abstractions;
using ResellioBackend.TicketPurchaseSystem.Results;
using ResellioBackend.TicketPurchaseSystem.Services.Abstractions;
using ResellioBackend.TicketPurchaseSystem.Statics;
using Stripe.Checkout;

namespace ResellioBackend.TicketPurchaseSystem.Services.Implementations
{
    public class StripePurchaseItemsCreatorService : IPurchaseItemCreatorService
    {
        private readonly ITicketsRepository _ticketsRepository;
        private readonly ICartRedisRepository _cartRedisRepository;

        public StripePurchaseItemsCreatorService(ICartRedisRepository cartRedisRepository, ITicketsRepository ticketsRepository)
        {
            _cartRedisRepository = cartRedisRepository;
            _ticketsRepository = ticketsRepository;
        }

        public async Task<PurchaseItemCreationResult> CreatePurchaseItemListAsync(int userId)
        {
            var ticketIdsEnumeration = await _cartRedisRepository.GetAllTicketsAsync(userId);
            if (ticketIdsEnumeration.Count() == 0)
            {
                return new PurchaseItemCreationResult()
                {
                    Success = false,
                    Message = "User has an empty cart"
                };
            }
            List<Ticket> ticketList = new List<Ticket>();
            foreach (var ticketId in ticketIdsEnumeration)
            {
                var ticket = await _ticketsRepository.GetTicketWithAllDependenciesByIdAsync(ticketId); // TODO: consider some ticket != null verification
                ticketList.Add(ticket!);                                                              
            }

            List<SessionLineItemOptions> lineItems = new List<SessionLineItemOptions>();
            foreach (var ticket in ticketList)
            {
                var price = ticket.GetPrice();
                if (price == null)
                {
                    return new PurchaseItemCreationResult()
                    {
                        Success = false,
                        Message = "Price is null, if we see this error something is seriously wrong in our DB"
                    };
                }

                var newItem = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)Math.Round(price.Amount * 100, MidpointRounding.AwayFromZero), // such complicated way of parsing is kind of redundancy, but I decided to leave extra protection layer, cause we deal with "money"
                        Currency = price.CurrencyCode.ToLower(),
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = ticket.TicketType.Event.Name,
                            Metadata = new Dictionary<string, string>
                            {
                                { CheckoutSessionMetadataKeys.TicketId, ticket.TicketId.ToString() }
                            }
                        }
                    },
                    Quantity = 1
                };
                lineItems.Add(newItem);

            }

            return new PurchaseItemCreationResult()
            {
                Success = true,
                Message = "",
                ItemList = lineItems
            };
        }
    }
}
