using System;
using System.Threading.Tasks;
using Moq;
using Xunit;
using ResellioBackend.EventManagementSystem.Creators.Implementations;
using ResellioBackend.EventManagementSystem.Models;
using ResellioBackend.EventManagementSystem.Models.Base;
using ResellioBackend.EventManagementSystem.Repositories.Abstractions;

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
        var ticketType = new TicketType();
        _ticketsRepositoryMock
            .Setup(repo => repo.AddAsync(It.IsAny<Ticket>()))
            .Returns(Task.CompletedTask)
            .Verifiable();

        // Act
        var result = await _ticketCreatorService.CreateTicketAsync(ticketType);

        // Assert
        Assert.True(result.Success);
        Assert.NotNull(result.Data);
        Assert.Equal(ticketType, result.Data.TicketType);
    }
}

