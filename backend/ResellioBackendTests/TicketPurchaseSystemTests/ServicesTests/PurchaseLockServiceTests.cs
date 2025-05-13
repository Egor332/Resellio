using Microsoft.EntityFrameworkCore.Storage;
using Moq;
using ResellioBackend.EventManagementSystem.Models.Base;
using ResellioBackend.EventManagementSystem.Repositories.Abstractions;
using ResellioBackend.TicketPurchaseSystem.RedisRepositories.Abstractions;
using ResellioBackend.TicketPurchaseSystem.RedisServices.Abstractions;
using ResellioBackend.TicketPurchaseSystem.Services.Implementations;
using ResellioBackend.TransactionManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResellioBackendTests.TicketPurchaseSystemTests.ServicesTests
{
    public class PurchaseLockServiceTests
    {
        private readonly Mock<ITicketsRepository> _mockTicketsRepository;
        private readonly Mock<IRedisService> _mockRedisService;
        private readonly Mock<IDatabaseTransactionManager> _mockTransactionManager;
        private readonly Mock<ICartCacheRepository> _mockCartRedisRepository;
        private readonly PurchaseLockService _purchaseLockService;

        public PurchaseLockServiceTests()
        {
            _mockTicketsRepository = new Mock<ITicketsRepository>();
            _mockRedisService = new Mock<IRedisService>();
            _mockTransactionManager = new Mock<IDatabaseTransactionManager>();
            _mockCartRedisRepository = new Mock<ICartCacheRepository>();

            var transactionMock = new Mock<IDbContextTransaction>();
            _mockTransactionManager.Setup(m => m.BeginTransactionAsync())
                                   .ReturnsAsync(transactionMock.Object);
            _mockTransactionManager.Setup(m => m.CommitTransactionAsync(It.IsAny<IDbContextTransaction>()))
                                   .Returns(Task.CompletedTask);
            _mockTransactionManager.Setup(m => m.RollbackTransactionAsync(It.IsAny<IDbContextTransaction>()))
                                   .Returns(Task.CompletedTask);

            _purchaseLockService = new PurchaseLockService(
                _mockTicketsRepository.Object,
                _mockRedisService.Object,
                _mockTransactionManager.Object,
                _mockCartRedisRepository.Object);
        }

        [Fact]
        public async Task EnsureEnoughLockTimeForPurchase_CartExpired_ReturnsFailure()
        {
            // Arrange
            int userId = 123;
            _mockCartRedisRepository.Setup(r => r.GetExpirationTimeAsync(userId))
                                    .ReturnsAsync((TimeSpan?)null);

            // Act
            var result = await _purchaseLockService.EnsureEnoughLockTimeForPurchaseAsync(userId);

            // Assert
            Assert.False(result.Success);
        }

        [Fact]
        public async Task EnsureEnoughLockTimeForPurchase_AlreadySufficientLockTime_ReturnsSuccess()
        {
            // Arrange
            int userId = 456;
            _mockCartRedisRepository.Setup(r => r.GetExpirationTimeAsync(userId))
                                    .ReturnsAsync(TimeSpan.FromMinutes(6));

            // Act
            var result = await _purchaseLockService.EnsureEnoughLockTimeForPurchaseAsync(userId);

            // Assert
            _mockRedisService.Verify(r => r.ChangeExpirationTimeForTicketAsync(It.IsAny<Guid>(), It.IsAny<TimeSpan>(), userId), Times.Never);
            _mockTicketsRepository.Verify(t => t.GetTicketByIdWithExclusiveRowLockAsync(It.IsAny<Guid>()), Times.Never);
            Assert.True(result.Success);
        }

        [Fact]
        public async Task EnsureEnoughLockTimeForPurchase_LockExtensionFails_ReturnsFailure()
        {
            // Arrange
            int userId = 789;
            _mockCartRedisRepository.Setup(r => r.GetExpirationTimeAsync(userId))
                                    .ReturnsAsync(TimeSpan.FromMinutes(3));

            var ticketId1 = Guid.NewGuid();
            var ticketId2 = Guid.NewGuid();
            var tickets = new List<Guid> { ticketId1, ticketId2 };

            _mockCartRedisRepository.Setup(r => r.GetAllTicketsAsync(userId))
                                    .ReturnsAsync(tickets);

            _mockRedisService.SetupSequence(r => r.ChangeExpirationTimeForTicketAsync(It.IsAny<Guid>(), It.IsAny<TimeSpan>(), userId))
                             .ReturnsAsync(true)
                             .ReturnsAsync(false);

            // Act
            var result = await _purchaseLockService.EnsureEnoughLockTimeForPurchaseAsync(userId);

            // Assert
            Assert.False(result.Success);
            _mockRedisService.Verify(r => r.UnlockTicketAsync(ticketId1, userId), Times.Once);
            _mockTicketsRepository.Verify(t => t.GetTicketByIdWithExclusiveRowLockAsync(It.IsAny<Guid>()), Times.Never);
        }

        [Fact]
        public async Task EnsureEnoughLockTimeForPurchase_DatabaseLockChangeThrowsExeption_InvokesRollback()
        {
            // Arrange
            int userId = 101;
            _mockCartRedisRepository.Setup(r => r.GetExpirationTimeAsync(userId))
                                    .ReturnsAsync(TimeSpan.FromMinutes(4));

            var ticketId = Guid.NewGuid();
            var ticketIds = new List<Guid> { ticketId };
            _mockCartRedisRepository.Setup(r => r.GetAllTicketsAsync(userId))
                                    .ReturnsAsync(ticketIds);
            _mockRedisService.Setup(r => r.ChangeExpirationTimeForTicketAsync(It.IsAny<Guid>(), It.IsAny<TimeSpan>(), userId))
                             .ReturnsAsync(true);

            var mockTicket = new Ticket { PurchaseIntenderId = userId };
            _mockTicketsRepository.Setup(t => t.GetTicketByIdWithExclusiveRowLockAsync(ticketId))
                                  .ReturnsAsync(mockTicket);

            _mockTransactionManager.Setup(m => m.CommitTransactionAsync(It.IsAny<IDbContextTransaction>()))
                                   .ThrowsAsync(new Exception("err"));

            // Act
            var result = await _purchaseLockService.EnsureEnoughLockTimeForPurchaseAsync(userId);

            // Assert
            Assert.False(result.Success);
            _mockTransactionManager.Verify(tm => tm.RollbackTransactionAsync(It.IsAny<IDbContextTransaction>()), Times.Once);
        }

        [Fact]
        public async Task EnsureEnoughLockTimeForPurchase_EverythingOK_EndsWithSuccessAndTransactionCommit()
        {
            // Arrange
            int userId = 101;
            _mockCartRedisRepository.Setup(r => r.GetExpirationTimeAsync(userId))
                                    .ReturnsAsync(TimeSpan.FromMinutes(4));

            var ticketId = Guid.NewGuid();
            var ticketIds = new List<Guid> { ticketId };
            _mockCartRedisRepository.Setup(r => r.GetAllTicketsAsync(userId))
                                    .ReturnsAsync(ticketIds);
            _mockRedisService.Setup(r => r.ChangeExpirationTimeForTicketAsync(It.IsAny<Guid>(), It.IsAny<TimeSpan>(), userId))
                             .ReturnsAsync(true);

            var mockTicket = new Ticket { PurchaseIntenderId = userId };
            _mockTicketsRepository.Setup(t => t.GetTicketByIdWithExclusiveRowLockAsync(ticketId))
                                  .ReturnsAsync(mockTicket);


            // Act
            var result = await _purchaseLockService.EnsureEnoughLockTimeForPurchaseAsync(userId);

            // Assert
            Assert.True(result.Success);
            _mockTransactionManager.Verify(tm => tm.CommitTransactionAsync(It.IsAny<IDbContextTransaction>()), Times.Once);
        }

        [Fact]
        public async Task RollbackAddedTimeAsync_UpdatesTicketsCorrectly()
        {
            // Arrange
            int userId = 202;
            var ticketId = Guid.NewGuid();
            var ticketIds = new List<Guid> { ticketId };
            _mockCartRedisRepository.Setup(r => r.GetExpirationTimeAsync(userId))
                                    .ReturnsAsync(TimeSpan.FromMinutes(2));
            _mockRedisService.Setup(r => r.ChangeExpirationTimeForTicketAsync(It.IsAny<Guid>(), It.IsAny<TimeSpan>(), userId))
                             .ReturnsAsync(true);

            var mockTicket = new Ticket { PurchaseIntenderId = userId };
            _mockTicketsRepository.Setup(t => t.GetTicketByIdWithExclusiveRowLockAsync(ticketId))
                                  .ReturnsAsync(mockTicket);

            // Act
            await _purchaseLockService.RollbackAddedTimeAsync(ticketIds, userId);

            // Assert
            Assert.NotEqual(mockTicket.PurchaseIntenderId, userId);
            _mockTransactionManager.Verify(m => m.BeginTransactionAsync(), Times.Once);
            _mockTransactionManager.Verify(m => m.CommitTransactionAsync(It.IsAny<IDbContextTransaction>()), Times.Once);
        }
    }
}
