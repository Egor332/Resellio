using Moq;
using ResellioBackend.EventManagementSystem.Models.Base;
using ResellioBackend.EventManagementSystem.Repositories.Abstractions;
using ResellioBackend.Results;
using ResellioBackend.TicketPurchaseSystem.DatabaseServices.Abstractions;
using ResellioBackend.TicketPurchaseSystem.RedisServices.Abstractions;
using ResellioBackend.TicketPurchaseSystem.Services.Implementations;
using ResellioBackend.TransactionManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResellioBackendTests.ShoppingCartManagementSystemTests.ServicesTests
{
    public class TicketUnlockerServiceTests
    {
        private readonly Mock<ITicketStatusService> _mockTicketStatusService;
        private readonly Mock<IRedisService> _mockRedisService;
        private readonly Mock<IDatabaseTransactionManager> _mockDatabaseTransactionManager;
        private readonly Mock<ITicketsRepository> _mockTicketsRepository;
        private readonly TicketUnlockerService _ticketUnlockerService;

        public TicketUnlockerServiceTests()
        {
            _mockTicketStatusService = new Mock<ITicketStatusService>();
            _mockRedisService = new Mock<IRedisService>();
            _mockDatabaseTransactionManager = new Mock<IDatabaseTransactionManager>();
            _mockTicketsRepository = new Mock<ITicketsRepository>();
            _ticketUnlockerService = new TicketUnlockerService(
                _mockTicketStatusService.Object,
                _mockRedisService.Object,
                _mockDatabaseTransactionManager.Object,
                _mockTicketsRepository.Object);
        }

        [Fact]
        public async Task UnlockTicketAsync_SuccessfulUnlock_ReturnsSuccess()
        {
            // Arrange
            int userId = 1;
            Guid ticketId = Guid.NewGuid();
            var mockTransaction = new Mock<IDbTransaction>();

            _mockRedisService.Setup(r => r.DeleteFromCartAsync(ticketId, userId)).Returns(Task.CompletedTask);
            _mockTicketsRepository.Setup(t => t.GetTicketByIdWithExclusiveRowLock(ticketId))
                                  .ReturnsAsync(new Ticket());
            _mockRedisService.Setup(r => r.UnlockTicketAsync(ticketId, userId))
                             .ReturnsAsync(new ResultBase { Success = true });
            _mockTicketStatusService.Setup(t => t.UnlockTicketInDbAsync(It.IsAny<Ticket>()))
                                    .ReturnsAsync(new ResultBase() { Success = true });

            // Act
            var result = await _ticketUnlockerService.UnlockTicketAsync(userId, ticketId);

            // Assert
            Assert.True(result.Success);
        }

        [Fact]
        public async Task UnlockTicketAsync_CacheUnlockFails_ReturnFailure()
        {
            // Arrange
            int userId = 1;
            Guid ticketId = Guid.NewGuid();
            var mockTransaction = new Mock<IDbTransaction>();

            _mockRedisService.Setup(r => r.DeleteFromCartAsync(ticketId, userId)).Returns(Task.CompletedTask);
            _mockTicketsRepository.Setup(t => t.GetTicketByIdWithExclusiveRowLock(ticketId))
                                  .ReturnsAsync(new Ticket());
            _mockRedisService.Setup(r => r.UnlockTicketAsync(ticketId, userId))
                             .ReturnsAsync(new ResultBase { Success = false, Message = "Cache unlock failed" });

            // Act
            var result = await _ticketUnlockerService.UnlockTicketAsync(userId, ticketId);

            // Assert
            Assert.False(result.Success);
        }
    }
}
