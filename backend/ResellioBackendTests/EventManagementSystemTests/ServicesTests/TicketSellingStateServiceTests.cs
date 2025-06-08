using Microsoft.EntityFrameworkCore.Storage;
using Moq;
using ResellioBackend.EventManagementSystem.DTOs;
using ResellioBackend.EventManagementSystem.Models.Base;
using ResellioBackend.EventManagementSystem.Repositories.Abstractions;
using ResellioBackend.EventManagementSystem.Services.Implementations;
using ResellioBackend.TransactionManager;
using ResellioBackend.UserManagementSystem.Models.Base;
using ResellioBackend.UserManagementSystem.Models.Users;
using ResellioBackend.UserManagementSystem.Repositories.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ResellioBackendTests.EventManagementSystemTests.ServicesTests
{
    public class TicketSellingStateServiceTests
    {
        private readonly Mock<IUsersRepository<UserBase>> _mockUserRepo;
        private readonly Mock<ITicketsRepository> _mockTicketRepo;
        private readonly Mock<IDatabaseTransactionManager> _mockTransactionManager;
        private readonly TicketSellingStateService _service;

        public TicketSellingStateServiceTests()
        {
            _mockUserRepo = new Mock<IUsersRepository<UserBase>>();
            _mockTicketRepo = new Mock<ITicketsRepository>();
            _mockTransactionManager = new Mock<IDatabaseTransactionManager>();

            _service = new TicketSellingStateService(
                _mockUserRepo.Object,
                _mockTicketRepo.Object,
                _mockTransactionManager.Object
            );
        }

        [Fact]
        public async Task ResellTicketAsync_WhenAllValid_ShouldSucceed()
        {
            // Arrange
            var ticketId = Guid.NewGuid();
            var userId = 123;
            var dto = new ResellDto { TicketId = ticketId, Price = 10, Currency = "PLN" };

            var user = new Customer()
            {
                UserId = userId,
                ConnectedSellingAccount = "111"
            };
            

            var ticket = new Ticket()
            {
                HolderId = userId,
            };

            _mockTicketRepo.Setup(r => r.GetTicketByIdAsync(ticketId)).ReturnsAsync(ticket);
            _mockUserRepo.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync(user);

            // Act
            var result = await _service.ResellTicketAsync(dto, userId);

            // Assert
            Assert.True(result.Success);
        }

        [Fact]
        public async Task ResellTicketAsync_WhenTicketNotFound_ShouldFail()
        {
            // Arrange
            var dto = new ResellDto { TicketId = Guid.NewGuid() };
            _mockTicketRepo.Setup(r => r.GetTicketByIdAsync(dto.TicketId)).ReturnsAsync((Ticket)null);

            // Act
            var result = await _service.ResellTicketAsync(dto, 123);

            // Assert
            Assert.False(result.Success);
        }


        [Fact]
        public async Task ResellTicketAsync_WhenUserNotFound_ShouldFail()
        {
            // Arrange
            var dto = new ResellDto { TicketId = Guid.NewGuid() };
            var userId = 123;
            var ticket = new Ticket { HolderId = userId };

            _mockTicketRepo.Setup(r => r.GetTicketByIdAsync(dto.TicketId)).ReturnsAsync(ticket);
            _mockUserRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((UserBase)null);

            // Act
            var result = await _service.ResellTicketAsync(dto, userId);

            // Assert
            Assert.False(result.Success);
        }

        [Fact]
        public async Task ResellTicketAsync_WhenUserNotHolder_ShouldFail()
        {
            // Arrange
            var dto = new ResellDto { TicketId = Guid.NewGuid() };
            var userId = 123;
            var ticket = new Ticket { HolderId = userId };

            _mockTicketRepo.Setup(r => r.GetTicketByIdAsync(dto.TicketId)).ReturnsAsync(ticket);
            _mockUserRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((UserBase)null);

            // Act
            var result = await _service.ResellTicketAsync(dto, userId);

            // Assert
            Assert.False(result.Success);
        }

        [Fact]
        public async Task ResellTicketAsync_WhenUserCannotSell_ShouldFail()
        {
            // Arrange
            var dto = new ResellDto { TicketId = Guid.NewGuid() };
            int userId = 123; 
            var ticket = new Ticket { HolderId = userId };            

            var user = new Customer()
            {
                ConnectedSellingAccount = "111"
            };

            _mockTicketRepo.Setup(r => r.GetTicketByIdAsync(dto.TicketId)).ReturnsAsync(ticket);
            _mockUserRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(user);

            // Act
            var result = await _service.ResellTicketAsync(dto, userId);

            // Assert
            Assert.False(result.Success);
        }

        [Fact]
        public async Task ResellTicketAsync_WhenCurrencyInvalid_ShouldFail()
        {
            // Arrange
            var dto = new ResellDto { TicketId = Guid.NewGuid(), Currency = "INVALID" };
            int userId = 123;
            
            var ticket = new Ticket() { HolderId = userId };

            var user = new Customer()
            {
                UserId = userId,
                ConnectedSellingAccount = "111"
            };

            _mockTicketRepo.Setup(r => r.GetTicketByIdAsync(dto.TicketId)).ReturnsAsync(ticket);
            _mockUserRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(user);

            // Act
            var result = await _service.ResellTicketAsync(dto, userId);

            // Assert
            Assert.False(result.Success);
        }

        [Fact]
        public async Task StopSellingTicket_WhenValid_ShouldSucceed()
        {
            // Arrange
            var ticketId = Guid.NewGuid();
            var userId = 123;

            var ticket = new Mock<Ticket>();
            var ticketObject = ticket.Object;
            ticketObject.HolderId = userId;

            var user = new Mock<UserBase>();
            var mockTransaction = new Mock<IDbContextTransaction>();

            _mockTicketRepo.Setup(r => r.GetTicketByIdWithExclusiveRowLockAsync(ticketId)).ReturnsAsync(ticketObject);
            _mockUserRepo.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync(user.Object);
            _mockTransactionManager.Setup(t => t.BeginTransactionAsync()).ReturnsAsync(mockTransaction.Object);

            // Act
            var result = await _service.StopSellingTicket(new StopSellingTicketDto { TicketId = ticketId }, userId);

            // Assert
            Assert.True(result.Success);
            _mockTransactionManager.Verify(t => t.CommitTransactionAsync(mockTransaction.Object), Times.Once);
        }

        [Fact]
        public async Task StopSellingTicket_WhenTicketNotFound_ShouldFail()
        {
            // Arrange
            var ticketId = Guid.NewGuid();
            var userId = 123;
            var transactionMock = new Mock<IDbContextTransaction>();

            _mockTicketRepo.Setup(r => r.GetTicketByIdWithExclusiveRowLockAsync(ticketId)).ReturnsAsync((Ticket)null);
            _mockTransactionManager.Setup(t => t.BeginTransactionAsync()).ReturnsAsync(transactionMock.Object);

            // Act
            var result = await _service.StopSellingTicket(new StopSellingTicketDto { TicketId = ticketId }, userId);

            // Assert
            Assert.False(result.Success);
        }

        [Fact]
        public async Task StopSellingTicket_WhenTicketNotOwned_ShouldFail()
        {
            // Arrange
            var ticketId = Guid.NewGuid();
            var userId = 123;
            var transactionMock = new Mock<IDbContextTransaction>();
            var ticket = new Mock<Ticket>();
            var ticketObject = ticket.Object;
            ticketObject.HolderId = 999;

            _mockTicketRepo.Setup(r => r.GetTicketByIdWithExclusiveRowLockAsync(ticketId)).ReturnsAsync(ticketObject);
            _mockTransactionManager.Setup(t => t.BeginTransactionAsync()).ReturnsAsync(transactionMock.Object);

            // Act
            var result = await _service.StopSellingTicket(new StopSellingTicketDto { TicketId = ticketId }, userId);

            // Assert
            Assert.False(result.Success);
        }



    }
}
