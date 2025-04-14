using ResellioBackend.TicketPurchaseSystem.Services.Abstractions;
using Stripe;
using Stripe.Checkout;

namespace ResellioBackend.TicketPurchaseSystem.Services.Implementations
{
    public class StripeCheckoutSessionManagerService : ICheckoutSessionManagerService
    {
        public async Task<List<Guid>?> GetTicketIdsOrNullFromSessionAsync(Session session)
        {
            var sessionLineItemsService = new SessionLineItemService();
            var lineItems = await sessionLineItemsService.ListAsync(session!.Id, new SessionLineItemListOptions
            {
                Limit = 20
            });
            var ticketIds = new List<Guid>();

            foreach (var item in lineItems.Data)
            {
                var price = item.Price;
                var productId = price.ProductId;

                if (!string.IsNullOrEmpty(productId))
                {
                    var productService = new ProductService();
                    var product = await productService.GetAsync(productId);

                    if (product.Metadata.TryGetValue("ticketId", out var ticketIdString)
                        && Guid.TryParse(ticketIdString, out var ticketId))
                    {
                        ticketIds.Add(ticketId);
                    }
                }
            }

            return ticketIds;
        }

        public int? GetUserIdOrNullFromSessionMetadata(Session session)
        {
            var userIdString = session.Metadata["userId"];
            int userId;
            var parseResult = int.TryParse(userIdString, out userId);
            if (parseResult) 
            {
                return userId;
            }
            else
            {
                return null;
            }
        }
    }
}
