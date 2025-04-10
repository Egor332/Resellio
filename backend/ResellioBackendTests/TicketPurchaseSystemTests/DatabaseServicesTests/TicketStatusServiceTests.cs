using Microsoft.EntityFrameworkCore.Storage;
using Moq;
using ResellioBackend.EventManagementSystem.Enums;
using ResellioBackend.EventManagementSystem.Models.Base;
using ResellioBackend.EventManagementSystem.Repositories.Abstractions;
using ResellioBackend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using ResellioBackend.TransactionManager;
using ResellioBackend.TicketPurchaseSystem.DatabaseServices.Implementations;
using ResellioBackend.UserManagementSystem.Models.Users;

namespace ResellioBackendTests.ShoppingCartManagementSystemTests.DatabaseServicesTests
{
    public class TicketStatusServiceTests
    {
        private readonly Mock<ITicketsRepository> _mockTicketsRepository;
        private readonly Mock<IDatabaseTransactionManager> _mockTransactionManager;
        private readonly TicketStatusService _ticketStatusService;

        public TicketStatusServiceTests()
        {
            _mockTicketsRepository = new Mock<ITicketsRepository>();
            _mockTransactionManager = new Mock<IDatabaseTransactionManager>();
            _ticketStatusService = new TicketStatusService(_mockTicketsRepository.Object, _mockTransactionManager.Object);
        }

        [Fact]
        public async Task LockTicketInDbAsync_TicketDoesNotExist_ReturnsFailure()
        {
            // Arrange
            var ticketId = Guid.NewGuid();
            _mockTicketsRepository.Setup(repo => repo.GetTicketByIdWithExclusiveRowLockAsync(ticketId))
                                  .ReturnsAsync((Ticket)null);

            // Act
            var result = await _ticketStatusService.LockTicketInDbAsync(ticketId, DateTime.UtcNow);

            // Assert
            Assert.False(result.Success);
        }

        [Fact]
        public async Task LockTicketInDbAsync_TicketAlreadyLocked_ReturnsFailure()
        {
            // Arrange
            var ticketId = Guid.NewGuid();
            var ticket = new Ticket { LastLock = DateTime.UtcNow.AddMinutes(5) };
            _mockTicketsRepository.Setup(repo => repo.GetTicketByIdWithExclusiveRowLockAsync(ticketId))
                                  .ReturnsAsync(ticket);

            // Act
            var result = await _ticketStatusService.LockTicketInDbAsync(ticketId, DateTime.UtcNow.AddMinutes(10));

            // Assert
            Assert.False(result.Success);
        }

        [Fact]
        public async Task LockTicketInDbAsync_TicketAlreadySold_ReturnsFailure()
        {
            // Arrange
            var ticketId = Guid.NewGuid();
            var ticket = new Ticket { LastLock = null, TicketState = TicketStates.Sold };
            _mockTicketsRepository.Setup(repo => repo.GetTicketByIdWithExclusiveRowLockAsync(ticketId))
                                  .ReturnsAsync(ticket);

            // Act
            var result = await _ticketStatusService.LockTicketInDbAsync(ticketId, DateTime.UtcNow.AddMinutes(10));

            // Assert
            Assert.False(result.Success);
        }

        [Fact]
        public async Task LockTicketInDbAsync_TicketCanBeLocked_ReturnsSuccess()
        {
            // Arrange
            var ticketId = Guid.NewGuid();
            var ticket = new Ticket { LastLock = null };
            _mockTicketsRepository.Setup(repo => repo.GetTicketByIdWithExclusiveRowLockAsync(ticketId))
                                  .ReturnsAsync(ticket);
            _mockTicketsRepository.Setup(repo => repo.UpdateAsync(ticket)).Returns(Task.CompletedTask);

            // Act
            var result = await _ticketStatusService.LockTicketInDbAsync(ticketId, DateTime.UtcNow.AddMinutes(10));

            // Assert
            Assert.True(result.Success);
            _mockTicketsRepository.Verify(tr => tr.UpdateAsync(ticket), Times.Once);

        }

        [Fact]
        public async Task UnlockTicketInDbAsync_TicketDoesNotExist_ReturnsFailure()
        {
            // Arrange

            // Act
            var result = await _ticketStatusService.UnlockTicketInDbAsync(null);

            // Assert
            Assert.False(result.Success);
        }

        [Fact]
        public async Task UnlockTicketInDbAsync_TicketCanBeUnlocked_ReturnsSuccess()
        {
            // Arrange
            var ticketId = Guid.NewGuid();
            var ticket = new Ticket { TicketState = TicketStates.Reserved, LastLock = DateTime.UtcNow };
            _mockTicketsRepository.Setup(repo => repo.UpdateAsync(ticket)).Returns(Task.CompletedTask);

            // Act
            var result = await _ticketStatusService.UnlockTicketInDbAsync(ticket);

            // Assert
            Assert.True(result.Success);
            _mockTicketsRepository.Verify(tr => tr.UpdateAsync(ticket), Times.Once);
        }

        [Fact]
        public async Task TryMarkAsSoledAsync_TicketAlreadySoled_ReturnsFailure()
        {
            // Arrange
            var ticketId = Guid.NewGuid();
            var ticket = new Ticket { TicketId = ticketId, TicketState = TicketStates.Sold };
            _mockTicketsRepository.Setup(repo => repo.GetTicketByIdWithExclusiveRowLockAsync(ticketId))
                .ReturnsAsync(ticket);

            // Act
            var result = await _ticketStatusService.TryMarkAsSoldAsync(ticketId, new Customer());

            // Assert
            Assert.False(result.Success);
        }

        [Fact]
        public async Task TryMarkAsSoledAsync_TicketReservedByDifferentUser_ReturnsFailure()
        {
            // Arrange
            var ticketId = Guid.NewGuid();
            var owner = new Customer { UserId = 1 };
            var differentOwner = new Customer { UserId = 2 };
            var ticket = new Ticket
            {
                TicketId = ticketId,
                TicketState = TicketStates.Reserved,
                LastLock = DateTime.UtcNow.AddMinutes(10),
                PurchaseIntenderId = differentOwner.UserId
            };
            _mockTicketsRepository.Setup(repo => repo.GetTicketByIdWithExclusiveRowLockAsync(ticketId))
                .ReturnsAsync(ticket);

            // Act
            var result = await _ticketStatusService.TryMarkAsSoldAsync(ticketId, owner);

            // Assert
            Assert.False(result.Success);
        }

        public async Task TryMarkAsSoledAsync_TicketReservedByThisUser_ReturnsSuccess()
        {
            // Arrange
            var ticketId = Guid.NewGuid();
            var owner = new Customer { UserId = 1 };
            var ticket = new Ticket
            {
                TicketId = ticketId,
                TicketState = TicketStates.Reserved,
                LastLock = DateTime.UtcNow.AddMinutes(10),
                PurchaseIntenderId = owner.UserId
            };
            _mockTicketsRepository.Setup(repo => repo.GetTicketByIdWithExclusiveRowLockAsync(ticketId))
                .ReturnsAsync(ticket);

            // Act
            var result = await _ticketStatusService.TryMarkAsSoldAsync(ticketId, owner);

            // Assert
            Assert.True(result.Success);
        }

        public async Task TryMarkAsSoledAsync_TicketReservedButLockExpired_ReturnsSuccess()
        {
            // Arrange
            var ticketId = Guid.NewGuid();
            var owner = new Customer { UserId = 1 };
            var ticket = new Ticket
            {
                TicketId = ticketId,
                TicketState = TicketStates.Reserved,
                LastLock = DateTime.UtcNow.AddMinutes(-5),
                PurchaseIntenderId = owner.UserId
            };
            _mockTicketsRepository.Setup(repo => repo.GetTicketByIdWithExclusiveRowLockAsync(ticketId))
                .ReturnsAsync(ticket);

            // Act
            var result = await _ticketStatusService.TryMarkAsSoldAsync(ticketId, owner);

            // Assert
            Assert.True(result.Success);
        }

        [Fact]
        public async Task TryMarkAsSoledAsync_TicketAvailableForSale_ReturnsSuccess()
        {
            // Arrange
            var ticketId = Guid.NewGuid();
            var owner = new Customer { UserId = 1 };
            var ticket = new Ticket { TicketId = ticketId, TicketState = TicketStates.Available };
            _mockTicketsRepository.Setup(repo => repo.GetTicketByIdWithExclusiveRowLockAsync(ticketId))
                .ReturnsAsync(ticket);

            // Act
            var result = await _ticketStatusService.TryMarkAsSoldAsync(ticketId, owner);

            // Assert
            Assert.True(result.Success);
            Assert.Equal(TicketStates.Sold, ticket.TicketState);
            Assert.Equal(owner, ticket.Holder);
        }

    }
}
