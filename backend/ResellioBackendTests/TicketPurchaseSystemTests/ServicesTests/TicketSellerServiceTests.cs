using Microsoft.EntityFrameworkCore.Storage;
using Moq;
using ResellioBackend.Results;
using ResellioBackend.TicketPurchaseSystem.DatabaseServices.Abstractions;
using ResellioBackend.TicketPurchaseSystem.Services.Implementations;
using ResellioBackend.TransactionManager;
using ResellioBackend.UserManagementSystem.Models.Users;
using StackExchange.Redis;
using System.Data;

namespace ResellioBackendTests.TicketPurchaseSystemTests.ServicesTests
{
    public class TicketSellerServiceTests
    {
        private readonly Mock<ITicketStatusService> _mockTicketStatusService;
        private readonly Mock<IDatabaseTransactionManager> _mockTransactionManager;
        private readonly TicketSellerService _ticketSellerService;

        public TicketSellerServiceTests()
        {
            _mockTicketStatusService = new Mock<ITicketStatusService>();
            _mockTransactionManager = new Mock<IDatabaseTransactionManager>();

            _ticketSellerService = new TicketSellerService(_mockTransactionManager.Object, _mockTicketStatusService.Object);
        }

        [Fact]
        public async Task TryMarkTicketsAsSoledAsync_AllTicketsSuccess_CommitsTransaction()
        {
            // Arrange
            var ticketIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };
            var buyer = new Customer { UserId = 1 };

            var mockTransaction = new Mock<IDbContextTransaction>().Object;

            _mockTransactionManager.Setup(tm => tm.BeginTransactionAsync())
                .ReturnsAsync(mockTransaction);

            _mockTicketStatusService.Setup(s => s.TryMarkAsSoldAsync(It.IsAny<Guid>(), buyer))
                .ReturnsAsync(new ResultBase { Success = true });

            // Act
            var result = await _ticketSellerService.TryMarkTicketsAsSoldAsync(ticketIds, buyer);

            // Assert
            Assert.True(result.Success);
            _mockTransactionManager.Verify(tm => tm.CommitTransactionAsync(mockTransaction), Times.Once);
            _mockTransactionManager.Verify(tm => tm.RollbackTransactionAsync(mockTransaction), Times.Never);
        }

        [Fact]
        public async Task TryMarkTicketsAsSoledAsync_OneTicketFails_RollsBackTransaction()
        {
            // Arrange
            var ticketIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };
            var buyer = new Customer { UserId = 1 };

            var mockTransaction = new Mock<IDbContextTransaction>().Object;

            _mockTransactionManager.Setup(tm => tm.BeginTransactionAsync())
                .ReturnsAsync(mockTransaction);

            _mockTicketStatusService.SetupSequence(s => s.TryMarkAsSoldAsync(It.IsAny<Guid>(), buyer))
                .ReturnsAsync(new ResultBase { Success = true }) // First ticket passes
                .ReturnsAsync(new ResultBase { Success = false}); // Second fails

            // Act
            var result = await _ticketSellerService.TryMarkTicketsAsSoldAsync(ticketIds, buyer);

            // Assert
            Assert.False(result.Success);
            _mockTransactionManager.Verify(tm => tm.RollbackTransactionAsync(mockTransaction), Times.Once);
            _mockTransactionManager.Verify(tm => tm.CommitTransactionAsync(mockTransaction), Times.Never);
        }
    }
}
