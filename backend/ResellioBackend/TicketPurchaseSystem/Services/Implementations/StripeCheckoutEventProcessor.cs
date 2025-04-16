using ResellioBackend.Results;
using ResellioBackend.TicketPurchaseSystem.Exceptions;
using ResellioBackend.TicketPurchaseSystem.Services.Abstractions;
using ResellioBackend.TicketPurchaseSystem.Statics;
using ResellioBackend.UserManagementSystem.Repositories.Abstractions;
using Stripe;
using Stripe.Checkout;

namespace ResellioBackend.TicketPurchaseSystem.Services.Implementations
{
    public class StripeCheckoutEventProcessor : ICheckoutEventProcessor
    {
        private readonly ITicketSellerService _ticketSellerService;
        private readonly IUsersRepository<UserManagementSystem.Models.Users.Customer> _customersRepository;
        private readonly ICheckoutSessionManagerService _checkoutSessionManagerService;
        private readonly IRefundService _refundService;

        public StripeCheckoutEventProcessor(ITicketSellerService ticketSellerService, 
            IUsersRepository<UserManagementSystem.Models.Users.Customer> customersRepository,
            ICheckoutSessionManagerService checkoutSessionManagerService,
            IRefundService refundService)
        {
            _ticketSellerService = ticketSellerService;
            _customersRepository = customersRepository;
            _checkoutSessionManagerService = checkoutSessionManagerService;
            _refundService = refundService;
        }

        public async Task<ResultBase> ProcessCheckoutEventAsync(Stripe.Event stripeEvent)
        {
            if (stripeEvent.Type == StripeEventTypes.CheckoutSessionCompleted)
            {
                var session = stripeEvent.Data.Object as Session;
                try
                {                    
                    var userId = _checkoutSessionManagerService.GetUserIdOrNullFromSessionMetadata(session);
                    if (userId == null)
                    {
                        throw new PurchaseException("No userId in metadata");
                    }
                    var ticketIds = await _checkoutSessionManagerService.GetTicketIdsOrNullFromSessionAsync(session);
                    if (ticketIds == null)
                    {
                        throw new PurchaseException("No tickets in metadata");
                    }

                    var buyer = await _customersRepository.GetByIdAsync(userId.Value);
                    if (buyer == null)
                    {
                        throw new PurchaseException("User with this Id does not exist");
                    }
                    var sellingResult = await _ticketSellerService.TryMarkTicketsAsSoldAsync(ticketIds, buyer);
                    if (!sellingResult.Success)
                    {
                        throw new PurchaseException("Can't sell tickets");
                    }
                    return new ResultBase()
                    {
                        Success = true,
                        Message = "sold"
                    };
                }
                catch (Exception ex)
                {
                    await _refundService.RefundPaymentAsync(session.PaymentIntentId, ex);
                    return new ResultBase()
                    {
                        Success = true,
                        Message = "Refunded"
                    };
                }
            }
            else
            {
                return new ResultBase()
                {
                    Success = true,
                    Message = "Nothing"
                };
            }
        }
    }
}
