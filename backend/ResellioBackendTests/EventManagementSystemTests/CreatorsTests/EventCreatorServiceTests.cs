using Moq;
using ResellioBackend.EventManagementSystem.Creators.Abstractions;
using ResellioBackend.EventManagementSystem.Creators.Implementations;
using ResellioBackend.EventManagementSystem.DTOs;
using ResellioBackend.EventManagementSystem.Models;
using ResellioBackend.EventManagementSystem.Repositories.Abstractions;
using ResellioBackend.Results;
using ResellioBackend.UserManagementSystem.Models.Base;
using ResellioBackend.UserManagementSystem.Models.Users;
using ResellioBackend.UserManagementSystem.Repositories.Abstractions;
using Xunit;

namespace ResellioBackendTests.EventManagementSystemTests.ServicesTests;

public class EventCreatorServiceTests
{
    private readonly EventCreatorService _eventCreatorService;
    private readonly Mock<ITicketTypeCreatorService> _ticketTypeCreatorServiceMock;
    private readonly Mock<IEventsRepository> _eventRepositoryMock;
    private readonly Mock<IUsersRepository<Organiser>> _userRepositoryMock;

    public EventCreatorServiceTests()
    {
        _ticketTypeCreatorServiceMock = new Mock<ITicketTypeCreatorService>();
        _eventRepositoryMock = new Mock<IEventsRepository>();
        _userRepositoryMock = new Mock<IUsersRepository<Organiser>>();
        _eventCreatorService = new EventCreatorService(_userRepositoryMock.Object, _eventRepositoryMock.Object, _ticketTypeCreatorServiceMock.Object);
    }
    
    [Fact]
    public async Task EventCreatorService_CreateEventAsync_ShouldReturnFailure_WhenOrganiserNotFound()
    {
        // Arrange
        int organiserId = 1;
        _userRepositoryMock
            .Setup(repo => repo.GetByIdAsync(organiserId))
            .ReturnsAsync((Organiser)null);

        var eventDto = new EventDto();

        // Act
        var result = await _eventCreatorService.CreateEventAsync(eventDto, organiserId);

        // Assert
        Assert.False(result.Success);
        // assure that AddAsync was never called on the repository during test execution
        _eventRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Event>()), Times.Never);
    }
    
    [Fact]
    public async Task CreateEventAsync_ValidEvent_ReturnsSuccessAndCreatesEvent()
    {
        // Arrange
        int organiserId = 1;
        var organiser = new Organiser();
        _userRepositoryMock
            .Setup(repo => repo.GetByIdAsync(organiserId))
            .ReturnsAsync(organiser);


        var eventDto = new EventDto
        {
            Name = "Awesome event",
            Description = "Awesome stuff's goin' on",
            Start = DateTime.Now,
            End = DateTime.Now.AddHours(3),
            TicketTypeDtos = "[{\"description\":\"1\",\"maxCount\":1,\"price\":13,\"currency\":\"PLN\",\"availableFrom\":\"2025-05-11T13:53:12.899Z\"},{\"description\":\"1\",\"maxCount\":1,\"price\":13,\"currency\":\"PLN\",\"availableFrom\":\"2025-05-11T13:53:12.899Z\"}]"
        };

        // Creating ticket types simulation
        _ticketTypeCreatorServiceMock
            .Setup(service => service.CreateTicketType(It.IsAny<TicketTypeDto>(), It.IsAny<Event>()))
            .Returns(new GeneralResult<TicketType>(){Success = true, Data = new TicketType()});

        // Act
        var result = await _eventCreatorService.CreateEventAsync(eventDto, organiserId);

        // Assert
        Assert.True(result.Success);

        // Verification of method calls 
        _userRepositoryMock.Verify(repo => repo.GetByIdAsync(organiserId), Times.Once);
        _ticketTypeCreatorServiceMock.Verify(service => service.CreateTicketType(It.IsAny<TicketTypeDto>(), It.IsAny<Event>()), Times.Exactly(2));
        _eventRepositoryMock.Verify(repo => repo.AddAsync(It.Is<Event>(e =>
            e.Organiser == organiser &&
            e.Name == eventDto.Name &&
            e.Description == eventDto.Description &&
            e.Start == eventDto.Start &&
            e.End == eventDto.End &&
            e.TicketTypes.Count == 2
        )), Times.Once);
    }
}
