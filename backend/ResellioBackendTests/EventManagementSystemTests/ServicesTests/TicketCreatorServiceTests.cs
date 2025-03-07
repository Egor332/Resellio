using System;
using System.Threading.Tasks;
using Moq;
using Xunit;
using ResellioBackend.EventManagementSystem.Creators.Implementations;
using ResellioBackend.EventManagementSystem.Models;
using ResellioBackend.EventManagementSystem.Models.Base;
using ResellioBackend.EventManagementSystem.Repositories.Abstractions;
using ResellioBackend.EventManagementSystem.Results;

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
    public async Task TicketCreatorService_CreateTicketAsync_ShouldReturnSuccessAndCallAddAsync()
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
        Assert.Equal("Created successfully", result.Message);
        Assert.NotNull(result.Data);
        Assert.Equal(ticketType, result.Data.TicketType);
        _ticketsRepositoryMock.Verify(repo => repo.AddAsync(It.Is<Ticket>(t => t.TicketType == ticketType)), Times.Once);
    }

    [Fact]
    public async Task TicketCreatorService_CreateTicketAsync_ShouldThrowExceptionIfRepositoryFails()
    {
        // Arrange
        var ticketType = new TicketType();
        _ticketsRepositoryMock
            .Setup(repo => repo.AddAsync(It.IsAny<Ticket>()))
            .ThrowsAsync(new Exception("Repository failure"));

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(async () =>
        {
            await _ticketCreatorService.CreateTicketAsync(ticketType);
        });
    }
}

