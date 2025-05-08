using Moq;
using ResellioBackend.EventManagementSystem.Models.Base;
using ResellioBackend.EventManagementSystem.Models;
using ResellioBackend.EventManagementSystem.Repositories.Abstractions;
using ResellioBackend.TicketPurchaseSystem.RedisRepositories.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ResellioBackend.TicketPurchaseSystem.Services.Implementations;
using ResellioBackend.TicketPurchaseSystem.Statics;

namespace ResellioBackendTests.TicketPurchaseSystemTests.ServicesTests
{
    public class StripePurchaseItemsCreatorServiceTests
    {
        private readonly Mock<ICartCacheRepository> _mockCartRedisRepo;
        private readonly Mock<ITicketsRepository> _mockTicketsRepo;
        private readonly StripePurchaseItemsCreatorService _service;

        public StripePurchaseItemsCreatorServiceTests()
        {
            _mockCartRedisRepo = new Mock<ICartCacheRepository>();
            _mockTicketsRepo = new Mock<ITicketsRepository>();
            _service = new StripePurchaseItemsCreatorService(_mockCartRedisRepo.Object, _mockTicketsRepo.Object);
        }

        [Fact]
        public async Task CreatePurchaseItemListAsync_UserHasEmptyCart_ReturnsFailure()
        {
            // Arrange
            int userId = 1;
            _mockCartRedisRepo.Setup(x => x.GetAllTicketsAsync(userId))
                .ReturnsAsync(Enumerable.Empty<Guid>());

            // Act
            var result = await _service.CreatePurchaseItemListAsync(userId);

            // Assert
            Assert.False(result.Success);
        }

        [Fact]
        public async Task CreatePurchaseItemListAsync_ValidTickets_ReturnsSuccessWithLineItems()
        {
            // Arrange
            int userId = 3;
            var ticket1Id = Guid.NewGuid();
            var ticket2Id = Guid.NewGuid();
            var ticketIds = new List<Guid> { ticket1Id, ticket2Id };

            var tickets = new List<Ticket>
            {
                new Ticket
                {
                    TicketId = ticket1Id,
                    TicketType = new TicketType
                    {
                        Event = new Event { Name = "Festival" }
                    },
                    CurrentPrice = new Money()
                    {
                        Amount = (decimal)10.00,
                        CurrencyCode = "USD"
                    }
                },
                new Ticket
                {
                    TicketId = ticket2Id,
                    TicketType = new TicketType
                    {
                        Event = new Event { Name = "Conference" }
                    },
                    CurrentPrice = new Money()
                    {
                        Amount = (decimal)10.00,
                        CurrencyCode = "USD"
                    }
                }
            };

            _mockCartRedisRepo.Setup(x => x.GetAllTicketsAsync(userId)).ReturnsAsync(ticketIds);
            _mockTicketsRepo.Setup(x => x.GetTicketWithAllDependenciesByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Guid id) => tickets.First(t => t.TicketId == id));

            // Act
            var result = await _service.CreatePurchaseItemListAsync(userId);

            // Assert
            Assert.True(result.Success);
            Assert.Equal(2, result.ItemList.Count);

            var expectedTicketIds = new List<string> { ticket1Id.ToString(), ticket2Id.ToString() };
            var actualTicketIds = result.ItemList
                .Select(item => item.PriceData.ProductData.Metadata[CheckoutSessionMetadataKeys.TicketId])
                .ToList();

            foreach (var id in expectedTicketIds)
            {
                Assert.Contains(id, actualTicketIds);
            }
        }
    }
}
