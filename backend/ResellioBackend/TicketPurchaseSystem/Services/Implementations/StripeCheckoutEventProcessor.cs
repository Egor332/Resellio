using ResellioBackend.Results;
using ResellioBackend.TicketPurchaseSystem.Services.Abstractions;
using ResellioBackend.TicketPurchaseSystem.Statics;
using ResellioBackend.UserManagementSystem.Repositories.Abstractions;
using Stripe;
using Stripe.Checkout;
using Stripe.TestHelpers;

namespace ResellioBackend.TicketPurchaseSystem.Services.Implementations
{
    public class StripeCheckoutEventProcessor : ICheckoutEventProcessor
    {
        private readonly ITicketSellerService _ticketSellerService;
        private readonly IUsersRepository<UserManagementSystem.Models.Users.Customer> _customersRepository;

        public StripeCheckoutEventProcessor(ITicketSellerService ticketSellerService, IUsersRepository<UserManagementSystem.Models.Users.Customer> customersRepository)
        {
            _ticketSellerService = ticketSellerService;
            _customersRepository = customersRepository;
        }

        public async Task<ResultBase> ProcessCheckoutEventAsync(Stripe.Event stripeEvent)
        {
            if (stripeEvent.Type == StripeEventTypes.CheckoutSessionCompleted)
            {
                var session = stripeEvent.Data.Object as Session;
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

                        if (product.Metadata.TryGetValue("ticketId", out var ticketIdStr)
                            && Guid.TryParse(ticketIdStr, out var ticketId))
                        {
                            ticketIds.Add(ticketId);
                        }
                    }
                }
                var userIdstring = session.Metadata["userId"];
                var userId = int.Parse(userIdstring);
                var buyer = await _customersRepository.GetByIdAsync(userId);
                if (buyer != null)
                {
                    var sellingResult = await _ticketSellerService.TryMarkTicketsAsSoldAsync(ticketIds, buyer);
                    if (sellingResult.Success)
                    {
                        return new ResultBase()
                        {
                            Success = true,
                            Message = "soled"
                        };
                    }
                }
                var refundService = new Stripe.RefundService();
                await refundService.CreateAsync(new RefundCreateOptions
                {
                    PaymentIntent = session.PaymentIntentId,
                    Reason = "lock_expired",
                    Metadata = new Dictionary<string, string>
                    {
                        { "reason", "ticket lock expired" },
                        { "userId", session.Metadata["userId"] }
                    }
                });
                return new ResultBase()
                {
                    Success = true,
                    Message = "Refunded"
                };
            }
            else
            {
                return new ResultBase()
                {
                    Success = true,
                    Message = "Nothing"
                };
            }
            throw new NotImplementedException();
        }
    }
}
