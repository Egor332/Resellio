using System;
using System.Threading.Tasks;
using Moq;
using Xunit;
using ResellioBackend.EventManagementSystem.Creators.Implementations;
using ResellioBackend.EventManagementSystem.Models;
using ResellioBackend.EventManagementSystem.Models.Base;
using ResellioBackend.EventManagementSystem.Repositories.Abstractions;
using ResellioBackend.UserManagementSystem.Models.Users;

namespace ResellioBackendTests.EventManagementSystemTests.ServicesTests;

public class TicketCreatorServiceTests
{
    private readonly TicketCreatorService _ticketCreatorService;
    private readonly Mock<ITicketsRepository> _ticketsRepositoryMock;

    public TicketCreatorServiceTests()
    {
        _ticketsRepositoryMock = new Mock<ITicketsRepository>();
        _ticketCreatorService = new TicketCreatorService(_ticketsRepositoryMock.Object);
    }

    [Fact]
    public async Task TicketCreatorService_CreateTicketAsync_ShouldReturnSuccessAndReturnTicket()
    {
        // Arrange
        var ticketPrice = new Money();
        var ticketType = new TicketType() { BasePrice = ticketPrice };
        ticketType.Event = new Event();
        ticketType.Event.Organiser = new Organiser();
        _ticketsRepositoryMock
            .Setup(repo => repo.AddAsync(It.IsAny<Ticket>()))
            .Returns(Task.CompletedTask)
            .Verifiable();

        // Act
        var result = _ticketCreatorService.CreateTicket(ticketType);

        // Assert
        Assert.True(result.Success);
        Assert.NotNull(result.Data);
        Assert.Equal(ticketType, result.Data.TicketType);
        Assert.Equal(ticketPrice.Amount, result.Data.CurrentPrice.Amount);
        Assert.Equal(ticketPrice.CurrencyCode, result.Data.CurrentPrice.CurrencyCode);
    }
}

