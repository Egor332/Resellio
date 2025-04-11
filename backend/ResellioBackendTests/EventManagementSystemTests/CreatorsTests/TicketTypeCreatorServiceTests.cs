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
    public void CreateTicketType_WhenCurrencyIsNotLegit_ShouldReturnFailure()
    {
        // Arrange
        var ticketTypeDto = new TicketTypeDto
        {
            Description = "Test Ticket Type",
            MaxCount = 3,
            Price = 50,
            Currency = "Not currency",
            AvailableFrom = DateTime.Now
        };

        var createdEvent = new Event();

        // Act
        var result = _ticketTypeCreatorService.CreateTicketType(ticketTypeDto, createdEvent);

        // Assert
        Assert.False(result.Success);
    }

    [Fact]
    public void CreateTicketType_WhenAllTicketsCreatedSuccessfully_ShouldReturnSuccessAndTicketType()
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

        _ticketCreatorServiceMock
            .Setup(service => service.CreateTicket(It.IsAny<TicketType>()))
            .Returns(() => new GeneralResult<Ticket>
            {
                Success = true,
                Data = new Ticket()
            });

        // Act
        var result = _ticketTypeCreatorService.CreateTicketType(ticketTypeDto, createdEvent);

        // Assert
        Assert.True(result.Success);
        Assert.NotNull(result.Data);
        Assert.Equal(ticketTypeDto.MaxCount, result.Data.Tickets.Count);
        _ticketCreatorServiceMock.Verify(service => service.CreateTicket(It.IsAny<TicketType>()), Times.Exactly(ticketTypeDto.MaxCount));
    }

    [Fact]
    public void CreateTicketType_WhenTicketCreationFails_ShouldReturnFailure()
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
            .Setup(service => service.CreateTicket(It.IsAny<TicketType>()))
            .Returns(() =>
            {
                callCount++;
                if (callCount == 2)
                {
                    return new GeneralResult<Ticket>
                    {
                        Success = false,
                    };
                }
                return new GeneralResult<Ticket>
                {
                    Success = true,
                    Data = new Ticket()
                };
            });

        // Act
        var result = _ticketTypeCreatorService.CreateTicketType(ticketTypeDto, createdEvent);

        // Assert
        Assert.False(result.Success);
        _ticketTypesRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<TicketType>()), Times.Never);
    }
}