using Moq;
using ResellioBackend.EventManagementSystem.Creators.Abstractions;
using ResellioBackend.EventManagementSystem.Creators.Implementations;
using ResellioBackend.EventManagementSystem.DTOs;
using ResellioBackend.EventManagementSystem.Models;
using ResellioBackend.EventManagementSystem.Models.Base;
using ResellioBackend.EventManagementSystem.Repositories.Abstractions;
using ResellioBackend.Results;

namespace ResellioBackendTests.EventManagementSystemTests.ServicesTests;

public class TicketTypeCreatorServiceTests
{

    private readonly TicketTypeCreatorService _ticketTypeCreatorService;
    private readonly Mock<ITicketTypesRepository> _ticketTypesRepositoryMock;
    private readonly Mock<ITicketCreatorService> _ticketCreatorServiceMock;

    public TicketTypeCreatorServiceTests()
    {
        _ticketTypesRepositoryMock = new Mock<ITicketTypesRepository>();
        _ticketCreatorServiceMock = new Mock<ITicketCreatorService>();
        _ticketTypeCreatorService = new TicketTypeCreatorService(_ticketTypesRepositoryMock.Object, _ticketCreatorServiceMock.Object);
    }

    [Fact]
    public async Task TicketTypeCreatorService_CreateTicketTypeAsync_ShouldReturnSuccessAndAddTicketType_WhenAllTicketsCreatedSuccessfully()
    {
        // Arrange
        var ticketTypeDto = new TicketTypeDto
        {
            Description = "Test Ticket Type",
            MaxCount = 3,
            Price = 50,
            Currency = "USD",
            AvailableFrom = DateTime.Now
        };

        var createdEvent = new Event();

        // Simulation setup: each call to CreateTicketAsync returns success with a new ticket.
        _ticketCreatorServiceMock
            .Setup(service => service.CreateTicketAsync(It.IsAny<TicketType>()))
            .ReturnsAsync(() => new GeneralResult<Ticket>
            {
                Success = true,
                Data = new Ticket()
            });

        // Act
        var result = await _ticketTypeCreatorService.CreateTicketTypeAsync(ticketTypeDto, createdEvent);

        // Assert
        Assert.True(result.Success);
        Assert.Equal("Created successfully", result.Message);
        Assert.NotNull(result.Data);
        Assert.Equal(ticketTypeDto.MaxCount, result.Data.Tickets.Count);

        // Method calls verification
        _ticketCreatorServiceMock.Verify(service => service.CreateTicketAsync(It.IsAny<TicketType>()), Times.Exactly(ticketTypeDto.MaxCount));
        _ticketTypesRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<TicketType>()), Times.Once);
    }

    [Fact]
    public async Task TicketTypeCreatorService_CreateTicketTypeAsync_ShouldReturnFailure_WhenTicketCreationFails()
    {
        // Arrange
        var ticketTypeDto = new TicketTypeDto
        {
            Description = "Test Ticket Type",
            MaxCount = 3,
            Price = 50,
            Currency = "USD",
            AvailableFrom = DateTime.Now
        };

        var createdEvent = new Event();

        int callCount = 0;
        _ticketCreatorServiceMock
            .Setup(service => service.CreateTicketAsync(It.IsAny<TicketType>()))
            .ReturnsAsync(() =>
            {
                callCount++;
                // Simulate that the second ticket creation fails
                if (callCount == 2)
                {
                    return new GeneralResult<Ticket>
                    {
                        Success = false,
                        Message = "Failed to create all tickets"
                    };
                }
                return new GeneralResult<Ticket>
                {
                    Success = true,
                    Data = new Ticket()
                };
            });

        // Act
        var result = await _ticketTypeCreatorService.CreateTicketTypeAsync(ticketTypeDto, createdEvent);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("Failed to create all tickets", result.Message);
        // Verify that the repository was never called as nothing should be created
        _ticketTypesRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<TicketType>()), Times.Never);
    }
}