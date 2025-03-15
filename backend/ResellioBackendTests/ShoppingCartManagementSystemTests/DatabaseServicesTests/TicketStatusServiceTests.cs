using Microsoft.EntityFrameworkCore.Storage;
using Moq;
using ResellioBackend.EventManagementSystem.Enums;
using ResellioBackend.EventManagementSystem.Models.Base;
using ResellioBackend.EventManagementSystem.Repositories.Abstractions;
using ResellioBackend.ShoppingCartManagementSystem.DatabaseServices.Implementations;
using ResellioBackend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using ResellioBackend.TransactionManager;

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
            _mockTicketsRepository.Setup(repo => repo.GetTicketByIdWithExclusiveRowLock(ticketId))
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
            _mockTicketsRepository.Setup(repo => repo.GetTicketByIdWithExclusiveRowLock(ticketId))
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
            var ticket = new Ticket { LastLock = null, TicketState = TicketStates.Soled };
            _mockTicketsRepository.Setup(repo => repo.GetTicketByIdWithExclusiveRowLock(ticketId))
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
            _mockTicketsRepository.Setup(repo => repo.GetTicketByIdWithExclusiveRowLock(ticketId))
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
            var ticketId = Guid.NewGuid();
            _mockTicketsRepository.Setup(repo => repo.GetTicketByIdWithExclusiveRowLock(ticketId))
                                  .ReturnsAsync((Ticket)null);

            // Act
            var result = await _ticketStatusService.UnlockTicketInDbAsync(ticketId);

            // Assert
            Assert.False(result.Success);
        }

        [Fact]
        public async Task UnlockTicketInDbAsync_TicketCanBeUnlocked_ReturnsSuccess()
        {
            // Arrange
            var ticketId = Guid.NewGuid();
            var ticket = new Ticket { TicketState = TicketStates.Reserved, LastLock = DateTime.UtcNow };
            _mockTicketsRepository.Setup(repo => repo.GetTicketByIdWithExclusiveRowLock(ticketId))
                                  .ReturnsAsync(ticket);
            _mockTicketsRepository.Setup(repo => repo.UpdateAsync(ticket)).Returns(Task.CompletedTask);

            // Act
            var result = await _ticketStatusService.UnlockTicketInDbAsync(ticketId);

            // Assert
            Assert.True(result.Success);
            _mockTicketsRepository.Verify(tr => tr.UpdateAsync(ticket), Times.Once);
        }

    }
}
