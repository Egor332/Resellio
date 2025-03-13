using Moq;
using ResellioBackend.ShoppingCartManagementSystem.RedisRepositories.Abstractions;
using ResellioBackend.ShoppingCartManagementSystem.RedisServices.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResellioBackendTests.ShoppingCartManagementSystemTests.RedisServicesTests
{
    public class RedisServiceTests
    {
        private readonly Mock<ITicketRedisRepository> _mockTicketRepository;
        private readonly Mock<ICartRedisRepository> _mockCartRepository;
        private readonly RedisService _redisService;

        public RedisServiceTests()
        {
            _mockTicketRepository = new Mock<ITicketRedisRepository>();
            _mockCartRepository = new Mock<ICartRedisRepository>();
            _redisService = new RedisService(_mockTicketRepository.Object, _mockCartRepository.Object);
        }

        [Fact]
        public async Task AddToCartAsync_WhenCartExists_AddsTicketToCartAndSetsExpirationTime()
        {
            // Arrange
            var ticketId = Guid.NewGuid();
            var userId = 1;
            var expirationTime = TimeSpan.FromMinutes(10);

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
    }
}
