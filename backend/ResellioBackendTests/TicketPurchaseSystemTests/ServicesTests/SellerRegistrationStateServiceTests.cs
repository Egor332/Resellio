using Moq;
using ResellioBackend.TicketPurchaseSystem.RedisRepositories.Abstractions;
using ResellioBackend.TicketPurchaseSystem.Services.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResellioBackendTests.TicketPurchaseSystemTests.ServicesTests
{
    public class SellerRegistrationStateServiceTests
    {
        private readonly Mock<IStateCacheRepository> _mockStateCacheRepository;
        private readonly SellerRegistrationStateService _service;

        public SellerRegistrationStateServiceTests()
        {
            _mockStateCacheRepository = new Mock<IStateCacheRepository>();
            _service = new SellerRegistrationStateService(_mockStateCacheRepository.Object);
        }

        [Fact]
        public async Task ValidateStateAsync_WhenStateExistInCache_ShouldReturnUserId()
        {
            // Arrange
            string state = "valid-state";
            int expectedUserId = 123;
            _mockStateCacheRepository
                .Setup(repo => repo.GetUserIdAsync(state))
                .ReturnsAsync(expectedUserId.ToString());

            // Act
            var result = await _service.ValidateStateAsync(state);

            // Assert
            Assert.Equal(expectedUserId, result);
        }

        [Fact]
        public async Task ValidateStateAsync_WhenStateHasNoUserIds_ShouldReturnNull()
        {
            // Arrange
            string state = "empty-state";
            _mockStateCacheRepository
                .Setup(repo => repo.GetUserIdAsync(state))
                .ReturnsAsync("");

            // Act
            var result = await _service.ValidateStateAsync(state);

            // Assert
            Assert.Null(result);
        }
    }
}
