using Moq;
using ResellioBackend.TicketPurchaseSystem.RedisRepositories.Abstractions;
using ResellioBackend.TicketPurchaseSystem.RedisServices.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResellioBackendTests.ShoppingCartManagementSystemTests.RedisServicesTests
{
    public class RedisServiceTests
    {
        private readonly Mock<ITicketCacheRepository> _mockTicketRepository;
        private readonly Mock<ICartCacheRepository> _mockCartRepository;
        private readonly RedisService _redisService;

        public RedisServiceTests()
        {
            _mockTicketRepository = new Mock<ITicketCacheRepository>();
            _mockCartRepository = new Mock<ICartCacheRepository>();
            _redisService = new RedisService(_mockTicketRepository.Object, _mockCartRepository.Object);
        }

        [Fact]
        public async Task AddToCartAsync_WhenCartExists_AddsTicketToCartAndSetsExpirationTime()
        {
            // Arrange
            var ticketId = Guid.NewGuid();
            var userId = 1;
            var expirationTime = TimeSpan.FromMinutes(10);
            var timeOfExpiration = DateTime.UtcNow + expirationTime;

            _mockCartRepository.Setup(repo => repo.CheckCartForExistenceAsync(userId)).ReturnsAsync(true);
            _mockCartRepository.Setup(repo => repo.GetExpirationTimeAsync(userId)).ReturnsAsync(expirationTime);

            // Act
            await _redisService.AddToCartAsync(ticketId, userId);

            // Assert
            _mockCartRepository.Verify(repo => repo.AddTicketToCartAsync(userId, ticketId), Times.Once);
            _mockTicketRepository.Verify(repo => repo.SetExpirationTimeAsync(ticketId, expirationTime), Times.Once);
        }

        [Fact]
        public async Task AddToCartAsync_WhenCartDoesNotExist_AddsTicketToCartAndSetsExpirationTime()
        {
            // Arrange
            var ticketId = Guid.NewGuid();
            var userId = 1;
            var expirationTime = TimeSpan.FromMinutes(30);

            _mockCartRepository.Setup(repo => repo.CheckCartForExistenceAsync(userId)).ReturnsAsync(false);
            _mockTicketRepository.Setup(repo => repo.GetExpirationTimeAsync(ticketId)).ReturnsAsync(expirationTime);

            // Act
            await _redisService.AddToCartAsync(ticketId, userId);

            // Assert
            _mockCartRepository.Verify(repo => repo.AddTicketToCartAsync(userId, ticketId), Times.Once);
            _mockCartRepository.Verify(repo => repo.SetExpirationTimeAsync(userId, expirationTime), Times.Once);
        }

        [Fact]
        public async Task DeleteFromCartAsync_WhenCartBecomesEmpty_DeletesCart()
        {
            // Arrange
            var ticketId = Guid.NewGuid();
            var userId = 1;

            _mockCartRepository.Setup(repo => repo.GetCartLengthAsync(userId)).ReturnsAsync(0);

            // Act
            await _redisService.DeleteFromCartAsync(ticketId, userId);

            // Assert
            _mockCartRepository.Verify(repo => repo.DeleteTicketAsync(userId, ticketId), Times.Once);
            _mockCartRepository.Verify(repo => repo.DeleteCartAsync(userId), Times.Once);
        }

        [Fact]
        public async Task DeleteFromCartAsync_WhenCartIsNotEmpty_DoesNotDeleteCart()
        {
            // Arrange
            var ticketId = Guid.NewGuid();
            var userId = 1;

            _mockCartRepository.Setup(repo => repo.GetCartLengthAsync(userId)).ReturnsAsync(2);

            // Act
            await _redisService.DeleteFromCartAsync(ticketId, userId);

            // Assert
            _mockCartRepository.Verify(repo => repo.DeleteTicketAsync(userId, ticketId), Times.Once);
            _mockCartRepository.Verify(repo => repo.DeleteCartAsync(userId), Times.Never);
        }

        [Fact]
        public async Task UnlockTicketAsync_WhenLockedByAnotherUser_EndsWithFailure()
        {
            // Arrange
            var ticketId = Guid.NewGuid();
            var lockerId = 1;
            var userId = 2;

            _mockTicketRepository.Setup(repo => repo.GetUserIdAsync(ticketId)).ReturnsAsync(lockerId);

            // Act
            var result = await _redisService.UnlockTicketAsync(ticketId, userId);

            // Assert
            Assert.False(result.Success);
        }

        [Fact]
        public async Task UnlockTicketAsync_WhenLockedByCurrentUser_EndsWithSuccess()
        {
            // Arrange
            var ticketId = Guid.NewGuid();
            var userId = 2;

            _mockTicketRepository.Setup(repo => repo.GetUserIdAsync(ticketId)).ReturnsAsync(userId);

            // Act
            var result = await _redisService.UnlockTicketAsync(ticketId, userId);

            // Assert
            Assert.True(result.Success);
        }


        [Fact]
        public async Task ChangeExpirationTimeForTicketAsync_WhenUserIsNotLocker_ShouldReturnFalse()
        {
            // Arrange
            var ticketId = Guid.NewGuid();
            var userId = 1;
            var lockerId = 2;
            var expirationTime = TimeSpan.FromMinutes(30);

            _mockTicketRepository.Setup(r => r.GetUserIdAsync(ticketId))
                .ReturnsAsync(lockerId);

            // Act
            var result = await _redisService.ChangeExpirationTimeForTicketAsync(ticketId, expirationTime, userId);

            // Assert
            Assert.False(result);
            _mockTicketRepository.Verify(r => r.SetExpirationTimeAsync(It.IsAny<Guid>(), It.IsAny<TimeSpan>()), Times.Never);
        }

        [Fact]
        public async Task ChangeExpirationTimeForTicketAsync_WhenSetExpirationSucceeds_ShouldReturnTrue()
        {
            // Arrange
            var ticketId = Guid.NewGuid();
            var userId = 1;
            TimeSpan expirationTime = TimeSpan.FromMinutes(30);

            _mockTicketRepository.Setup(r => r.GetUserIdAsync(ticketId))
                .ReturnsAsync((int?)userId);

            _mockTicketRepository.Setup(r => r.SetExpirationTimeAsync(ticketId, expirationTime))
                .ReturnsAsync(true);

            // Act
            var result = await _redisService.ChangeExpirationTimeForTicketAsync(ticketId, expirationTime, userId);

            // Assert
            Assert.True(result);
            _mockTicketRepository.Verify(r => r.LockTicketAsync(It.IsAny<Guid>(), It.IsAny<TimeSpan>(), It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task ChangeExpirationTimeForTicketAsync_WhenSetExpirationFailsButLockSucceeds_ShouldReturnTrue()
        {
            // Arrange
            var ticketId = Guid.NewGuid();
            var userId = 1;
            TimeSpan expirationTime = TimeSpan.FromMinutes(30);

            _mockTicketRepository.Setup(r => r.GetUserIdAsync(ticketId))
                .ReturnsAsync((int?)userId);

            _mockTicketRepository.Setup(r => r.SetExpirationTimeAsync(ticketId, expirationTime))
                .ReturnsAsync(false);

            _mockTicketRepository.Setup(r => r.LockTicketAsync(ticketId, expirationTime, userId))
                .ReturnsAsync(true);

            // Act
            var result = await _redisService.ChangeExpirationTimeForTicketAsync(ticketId, expirationTime, userId);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task ChangeExpirationTimeForTicketAsync_WhenBothSetAndLockFail_ShouldReturnFalse()
        {
            // Arrange
            var ticketId = Guid.NewGuid();
            var userId = 1;
            TimeSpan expirationTime = TimeSpan.FromMinutes(30);

            _mockTicketRepository.Setup(r => r.GetUserIdAsync(ticketId))
                .ReturnsAsync((int?)userId);

            _mockTicketRepository.Setup(r => r.SetExpirationTimeAsync(ticketId, expirationTime))
                .ReturnsAsync(false);

            _mockTicketRepository.Setup(r => r.LockTicketAsync(ticketId, expirationTime, userId))
                .ReturnsAsync(false);

            // Act
            var result = await _redisService.ChangeExpirationTimeForTicketAsync(ticketId, expirationTime, userId);

            // Assert
            Assert.False(result);
        }
    }
}
