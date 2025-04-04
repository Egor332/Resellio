using Moq;
using ResellioBackend.TicketPurchaseSystem.Results;
using ResellioBackend.TicketPurchaseSystem.DatabaseServices.Abstractions;
using ResellioBackend.TicketPurchaseSystem.RedisServices.Abstractions;
using ResellioBackend.TicketPurchaseSystem.Services.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResellioBackendTests.ShoppingCartManagementSystemTests.ServicesTests
{
    public class TicketLockerServiceTests
    {
        private readonly Mock<ITicketStatusService> _mockTicketStatusService;
        private readonly Mock<IRedisService> _mockRedisService;
        private readonly TicketLockerService _ticketLockerService;

        public TicketLockerServiceTests()
        {
            _mockTicketStatusService = new Mock<ITicketStatusService>();
            _mockRedisService = new Mock<IRedisService>();
            _ticketLockerService = new TicketLockerService(_mockTicketStatusService.Object, _mockRedisService.Object);
        }

        [Fact]
        public async Task LockTicketAsync_TicketAlreadyLocked_ReturnsFailure()
        {
            // Arrange
            int userId = 1;
            Guid ticketId = Guid.NewGuid();
            _mockRedisService.Setup(r => r.InstantTicketLockAsync(ticketId, It.IsAny<TimeSpan>(), userId))
                             .ReturnsAsync(false);

            // Act
            var result = await _ticketLockerService.LockTicketAsync(userId, ticketId);

            // Assert
            Assert.False(result.Success);
        }

        [Fact]
        public async Task LockTicketAsync_DatabaseLockFails_ReturnsFailureAndRevertsRedis()
        {
            // Arrange
            int userId = 1;
            Guid ticketId = Guid.NewGuid();
            DateTime cartExpiration = DateTime.UtcNow.AddMinutes(10);

            _mockRedisService.Setup(r => r.InstantTicketLockAsync(ticketId, It.IsAny<TimeSpan>(), userId))
                             .ReturnsAsync(true);
            _mockRedisService.Setup(r => r.AddToCartAsync(ticketId, userId))
                             .ReturnsAsync(cartExpiration);
            _mockTicketStatusService.Setup(t => t.LockTicketInDbAsync(ticketId, cartExpiration))
                                    .ReturnsAsync(new TicketLockResult { Success = false, Message = "DB Lock Failed" });

            // Act
            var result = await _ticketLockerService.LockTicketAsync(userId, ticketId);

            // Assert
            Assert.False(result.Success);

            _mockRedisService.Verify(r => r.DeleteFromCartAsync(ticketId, userId), Times.Once);
            _mockRedisService.Verify(r => r.UnlockTicketAsync(ticketId, userId), Times.Once);
        }

        [Fact]
        public async Task LockTicketAsync_SuccessfulLock_ReturnsSuccess()
        {
            // Arrange
            int userId = 1;
            Guid ticketId = Guid.NewGuid();
            DateTime cartExpiration = DateTime.UtcNow.AddMinutes(10);

            _mockRedisService.Setup(r => r.InstantTicketLockAsync(ticketId, It.IsAny<TimeSpan>(), userId))
                             .ReturnsAsync(true);
            _mockRedisService.Setup(r => r.AddToCartAsync(ticketId, userId))
                             .ReturnsAsync(cartExpiration);
            _mockTicketStatusService.Setup(t => t.LockTicketInDbAsync(ticketId, cartExpiration))
                                    .ReturnsAsync(new TicketLockResult { Success = true });

            // Act
            var result = await _ticketLockerService.LockTicketAsync(userId, ticketId);

            // Assert
            Assert.True(result.Success);
            Assert.Equal(ticketId, result.TicketId);
        }
    }
}
