using Microsoft.Extensions.Configuration;
using Moq;
using ResellioBackend.Results;
using ResellioBackend.TicketPurchaseSystem.Results;
using ResellioBackend.TicketPurchaseSystem.Services.Abstractions;
using ResellioBackend.TicketPurchaseSystem.Services.Implementations;
using Stripe.Checkout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResellioBackendTests.TicketPurchaseSystemTests.ServicesTests
{
    public class StripeCheckoutSessionCreatorServiceTests
    {
        private readonly Mock<IPurchaseItemCreatorService> _mockPurchaseItemCreatorService;
        private readonly Mock<IPurchaseLockService> _mockPurchaseLockService;
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly StripeCheckoutSessionCreatorService _checkoutSessionCreatorService;

        public StripeCheckoutSessionCreatorServiceTests()
        {
            _mockPurchaseItemCreatorService = new Mock<IPurchaseItemCreatorService>();
            _mockPurchaseLockService = new Mock<IPurchaseLockService>();
            _mockConfiguration = new Mock<IConfiguration>();

            _mockConfiguration.Setup(config => config["FrontEndLinks:PaymentSuccess"])
                .Returns("https://success.com");
            _mockConfiguration.Setup(config => config["FrontEndLinks:PaymentCancel"])
                .Returns("https://cancel.com");

            _checkoutSessionCreatorService = new StripeCheckoutSessionCreatorService(
                _mockPurchaseItemCreatorService.Object,
                _mockPurchaseLockService.Object,
                _mockConfiguration.Object
            );
        }

        [Fact]
        public async Task CreateCheckoutSessionAsync_WhenLineItemCreationFails_ReturnsFailure()
        {
            // Arrange
            var userId = 1;
            int sellerId = 2;
            _mockPurchaseItemCreatorService
                .Setup(s => s.CreatePurchaseItemListAsync(userId, sellerId))
                .ReturnsAsync(new PurchaseItemCreationResult { Success = false, Message = "Line item error" });

            // Act
            var result = await _checkoutSessionCreatorService.CreateCheckoutSessionAsync(userId, sellerId);

            // Assert
            Assert.False(result.Success);
        }

        [Fact]
        public async Task CreateCheckoutSessionAsync_WhenLockExtensionFails_ReturnsFailure()
        {
            // Arrange
            var userId = 1;
            var sellerId = 2;
            _mockPurchaseItemCreatorService
                .Setup(s => s.CreatePurchaseItemListAsync(userId, sellerId))
                .ReturnsAsync(new PurchaseItemCreationResult
                {
                    Success = true,
                    ItemList = new List<SessionLineItemOptions>()
                });

            _mockPurchaseLockService
                .Setup(s => s.EnsureEnoughLockTimeForPurchaseAsync(userId))
                .ReturnsAsync(new ResultBase { Success = false, Message = "Lock failed" });

            // Act
            var result = await _checkoutSessionCreatorService.CreateCheckoutSessionAsync(userId, sellerId);

            // Assert
            Assert.False(result.Success);
        }
    }
}
