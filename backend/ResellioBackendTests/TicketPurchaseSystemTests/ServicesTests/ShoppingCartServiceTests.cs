using Moq;
using ResellioBackend.EventManagementSystem.Models;
using ResellioBackend.EventManagementSystem.Models.Base;
using ResellioBackend.EventManagementSystem.Repositories.Abstractions;
using ResellioBackend.TicketPurchaseSystem.RedisRepositories.Abstractions;
using ResellioBackend.TicketPurchaseSystem.Services.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResellioBackendTests.TicketPurchaseSystemTests.ServicesTests
{
    public class ShoppingCartServiceTests
    {
        private readonly Mock<ICartCacheRepository> _mockCartRepository;
        private readonly Mock<ITicketsRepository> _mockTicketsRepository;
        private readonly ShoppingCartService _shoppingCartService;

        public ShoppingCartServiceTests()
        {
            _mockCartRepository = new Mock<ICartCacheRepository>();
            _mockTicketsRepository = new Mock<ITicketsRepository>();
            _shoppingCartService = new ShoppingCartService(_mockCartRepository.Object, _mockTicketsRepository.Object);
        }

        [Fact]
        public async Task GetShoppingCartInfoAsync_WhenCartHasTickets_ReturnsCartInfo()
        {
            // Arrange
            int userId = 1;
            var ticketIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };
            var expirationTime = TimeSpan.FromMinutes(30);

            _mockCartRepository.Setup(repo => repo.GetExpirationTimeAsync(userId))
                               .ReturnsAsync(expirationTime);
            _mockCartRepository.Setup(repo => repo.GetAllTicketsAsync(userId))
                               .ReturnsAsync(ticketIds);

            foreach (var ticketId in ticketIds)
            {
                var ticket = new Ticket 
                { 
                    TicketId = ticketId,
                    TicketType = new TicketType() { Event = new Event() },
                    CurrentPrice = new Money()
                    {
                        CurrencyCode = "USD",
                        Amount = 10,
                    },
                    HolderId = 1,                   
                }; 
                _mockTicketsRepository.Setup(repo => repo.GetTicketWithAllDependenciesByIdAsync(ticketId))
                                      .ReturnsAsync(ticket);
            }

            // Act
            var result = await _shoppingCartService.GetShoppingCartInfoAsync(userId);

            // Assert
            Assert.True(result.IsCartExist);
            Assert.NotNull(result.ticketsInCart);
            Assert.Equal(ticketIds.Count, result.ticketsInCart.Count);
        }

        [Fact]
        public async Task GetShoppingCartInfoAsync_WhenCartIsEmpty_ReturnsNonExistentCart()
        {
            // Arrange
            int userId = 2;
            var emptyTicketIds = new List<Guid>();
            var expirationTime = TimeSpan.FromMinutes(10);

            _mockCartRepository.Setup(repo => repo.GetExpirationTimeAsync(userId))
                               .ReturnsAsync(expirationTime);
            _mockCartRepository.Setup(repo => repo.GetAllTicketsAsync(userId))
                               .ReturnsAsync(emptyTicketIds);

            // Act
            var result = await _shoppingCartService.GetShoppingCartInfoAsync(userId);

            // Assert
            Assert.False(result.IsCartExist);
            Assert.Null(result.ticketsInCart);
            Assert.Null(result.CartExpirationTime);
        }
    }
}
