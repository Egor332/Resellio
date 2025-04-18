using Moq;
using ResellioBackend.Results;
using ResellioBackend.TicketPurchaseSystem.Services.Abstractions;
using ResellioBackend.TicketPurchaseSystem.Services.Implementations;
using ResellioBackend.TicketPurchaseSystem.Statics;
using ResellioBackend.UserManagementSystem.Repositories.Abstractions;
using Stripe.Checkout;

namespace ResellioBackendTests.TicketPurchaseSystemTests.ServicesTests
{
    public class StripeCheckoutEventProcessorTests
    {
        private readonly Mock<ICheckoutSessionManagerService> _mockSessionManager;
        private readonly Mock<IUsersRepository<ResellioBackend.UserManagementSystem.Models.Users.Customer>> _mockCustomersRepo;
        private readonly Mock<ITicketSellerService> _mockTicketSeller;
        private readonly Mock<IRefundService> _mockRefundService;
        private readonly StripeCheckoutEventProcessor _service;

        public StripeCheckoutEventProcessorTests()
        {
            _mockSessionManager = new Mock<ICheckoutSessionManagerService>();
            _mockCustomersRepo = new Mock<IUsersRepository<ResellioBackend.UserManagementSystem.Models.Users.Customer>>();
            _mockTicketSeller = new Mock<ITicketSellerService>();
            _mockRefundService = new Mock<IRefundService>();

            _service = new StripeCheckoutEventProcessor(
                _mockTicketSeller.Object,
                _mockCustomersRepo.Object,
                _mockSessionManager.Object,
                _mockRefundService.Object
            );
        }

        [Fact]
        public async Task ProcessCheckoutEventAsync_WhenEventIsNotCheckoutSessionCompleted_ReturnsNothing()
        {
            var stripeEvent = new Stripe.Event { Type = "some.other.event" };

            var result = await _service.ProcessCheckoutEventAsync(stripeEvent);

            Assert.True(result.Success);
            Assert.Equal("Nothing", result.Message);
        }

        [Fact]
        public async Task ProcessCheckoutEventAsync_WhenUserIdIsNull_Refunds()
        {
            // Arrange
            var session = new Session { PaymentIntentId = "pi_123" };
            var stripeEvent = new Stripe.Event
            {
                Type = StripeEventTypes.CheckoutSessionCompleted,
                Data = new Stripe.EventData { Object = session }
            };

            _mockSessionManager.Setup(x => x.GetUserIdOrNullFromSessionMetadata(session)).Returns((int?)null);

            // Act
            var result = await _service.ProcessCheckoutEventAsync(stripeEvent);

            // Assert
            _mockRefundService.Verify(x => x.RefundPaymentAsync("pi_123", It.IsAny<Exception>()), Times.Once);
            Assert.True(result.Success);
        }

        [Fact]
        public async Task ProcessCheckoutEventAsync_WhenTicketIdsAreNull_Refunds()
        {
            // Arrange
            var session = new Session { PaymentIntentId = "pi_123" };
            var stripeEvent = new Stripe.Event
            {
                Type = StripeEventTypes.CheckoutSessionCompleted,
                Data = new Stripe.EventData { Object = session }
            };

            _mockSessionManager.Setup(x => x.GetUserIdOrNullFromSessionMetadata(session)).Returns(1);
            _mockSessionManager.Setup(x => x.GetTicketIdsOrNullFromSessionAsync(session)).ReturnsAsync((List<Guid>?)null);

            // Act
            var result = await _service.ProcessCheckoutEventAsync(stripeEvent);

            // Assert
            _mockRefundService.Verify(x => x.RefundPaymentAsync("pi_123", It.IsAny<Exception>()), Times.Once);
            Assert.Equal("Refunded", result.Message);
        }

        [Fact]
        public async Task ProcessCheckoutEventAsync_WhenBuyerIsNotFound_Refunds()
        {
            var session = new Session { PaymentIntentId = "pi_123" };
            var stripeEvent = new Stripe.Event
            {
                Type = StripeEventTypes.CheckoutSessionCompleted,
                Data = new Stripe.EventData { Object = session }
            };

            _mockSessionManager.Setup(x => x.GetUserIdOrNullFromSessionMetadata(session)).Returns(42);
            _mockSessionManager.Setup(x => x.GetTicketIdsOrNullFromSessionAsync(session)).ReturnsAsync(new List<Guid> { Guid.NewGuid(), Guid.NewGuid() });
            _mockCustomersRepo.Setup(x => x.GetByIdAsync(42)).ReturnsAsync((ResellioBackend.UserManagementSystem.Models.Users.Customer?)null);

            var result = await _service.ProcessCheckoutEventAsync(stripeEvent);

            _mockRefundService.Verify(x => x.RefundPaymentAsync("pi_123", It.IsAny<Exception>()), Times.Once);
            Assert.Equal("Refunded", result.Message);
        }

        [Fact]
        public async Task ProcessCheckoutEventAsync_WhenSellingFails_Refunds()
        {
            // Arrange
            var session = new Session { PaymentIntentId = "pi_123" };
            var stripeEvent = new Stripe.Event
            {
                Type = StripeEventTypes.CheckoutSessionCompleted,
                Data = new Stripe.EventData { Object = session }
            };

            _mockSessionManager.Setup(x => x.GetUserIdOrNullFromSessionMetadata(session)).Returns(42);
            _mockSessionManager.Setup(x => x.GetTicketIdsOrNullFromSessionAsync(session)).ReturnsAsync(new List<Guid> { Guid.NewGuid(), });
            _mockCustomersRepo.Setup(x => x.GetByIdAsync(42)).ReturnsAsync(new ResellioBackend.UserManagementSystem.Models.Users.Customer());
            _mockTicketSeller.Setup(x => x.TryMarkTicketsAsSoldAsync(It.IsAny<List<Guid>>(), It.IsAny<ResellioBackend.UserManagementSystem.Models.Users.Customer>()))
                             .ReturnsAsync(new ResultBase { Success = false });

            // Act
            var result = await _service.ProcessCheckoutEventAsync(stripeEvent);

            // Assert
            _mockRefundService.Verify(x => x.RefundPaymentAsync("pi_123", It.IsAny<Exception>()), Times.Once);
            Assert.Equal("Refunded", result.Message);
        }

        [Fact]
        public async Task ProcessCheckoutEventAsync_ReturnsSuccess_WhenEverythingSucceeds()
        {
            // Arrange
            var session = new Session { PaymentIntentId = "pi_123" };
            var stripeEvent = new Stripe.Event
            {
                Type = StripeEventTypes.CheckoutSessionCompleted,
                Data = new Stripe.EventData { Object = session }
            };

            _mockSessionManager.Setup(x => x.GetUserIdOrNullFromSessionMetadata(session)).Returns(42);
            _mockSessionManager.Setup(x => x.GetTicketIdsOrNullFromSessionAsync(session)).ReturnsAsync(new List<Guid> { Guid.NewGuid(), Guid.NewGuid() });
            _mockCustomersRepo.Setup(x => x.GetByIdAsync(42)).ReturnsAsync(new ResellioBackend.UserManagementSystem.Models.Users.Customer());
            _mockTicketSeller.Setup(x => x.TryMarkTicketsAsSoldAsync(It.IsAny<List<Guid>>(), 
                                    It.IsAny<ResellioBackend.UserManagementSystem.Models.Users.Customer>()))
                             .ReturnsAsync(new ResultBase { Success = true });

            // Act
            var result = await _service.ProcessCheckoutEventAsync(stripeEvent);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("sold", result.Message); // Might want to correct this to "sold"
        }
    }
}
