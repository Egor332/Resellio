using Moq;
using ResellioBackend.Common.Paging;
using ResellioBackend.EventManagementSystem.Filtering;
using ResellioBackend.EventManagementSystem.Repositories.Abstractions;
using ResellioBackend.EventManagementSystem.Services.Implementations;
using ResellioBackend.EventManagementSystem.Models;

namespace ResellioBackendTests.EventManagementSystemTests.ServicesTests
{
    public class EventServiceTests
    {
        private readonly Mock<IEventsRepository> _mockEventsRepository;
        private readonly Mock<IPaginationService> _mockPaginationService;
        private readonly EventService _eventService;

        public EventServiceTests()
        {
            _mockEventsRepository = new Mock<IEventsRepository>();
            _mockPaginationService = new Mock<IPaginationService>();
            _eventService = new EventService(_mockEventsRepository.Object, _mockPaginationService.Object);
        }

        [Fact]
        public async Task GetFiltratedEventsWithPagingAsync_WhenOk_ReturnsCorrectPaginationResult()
        {
            // Arrange
            var eventsQuery = new List<Event>
            {
                new Event
                {
                    EventId = 1,
                    Name = "Sample Event",
                    Description = "Sample Description",
                    Start = DateTime.UtcNow,
                    End = DateTime.UtcNow.AddHours(2)
                }
            }.AsQueryable();

            _mockEventsRepository.Setup(r => r.GetAllAsQueryable())
                                 .Returns(eventsQuery);

            var paginatedResult = new PaginationResult<Event>
            {
                Items = eventsQuery.ToList(),
                PageNumber = 1,
                TotalAmount = 1
            };

            _mockPaginationService.Setup(p => p.ApplyPagingAsync<Event>(It.IsAny<IQueryable<Event>>(), 1, 10))
                              .ReturnsAsync(paginatedResult);

            var filter = new EventsFilter();

            // Act
            var result = await _eventService.GetFiltratedEventsWithPagingAsync(filter);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result.Items);
            var dto = result.Items.First();
            Assert.Equal(paginatedResult.Items[0].EventId, dto.Id);
            Assert.Equal(paginatedResult.Items[0].Name, dto.Name);
            Assert.Equal(paginatedResult.Items[0].Description, dto.Description);
            Assert.Equal(paginatedResult.PageNumber, result.PageNumber);
            Assert.Equal(paginatedResult.TotalAmount, result.TotalAmount);
        }

        [Fact]
        public async Task GetFiltratedEventsWithPagingAsync_ThrowsArgumentOutOfRangeException_WhenPaginationThrowsArgumentOutOfRangeException()
        {
            // Arrange
            _mockEventsRepository.Setup(r => r.GetAllAsQueryable())
                                 .Returns(new List<Event>().AsQueryable());

            _mockPaginationService.Setup(p => p.ApplyPagingAsync<Event>(It.IsAny<IQueryable<Event>>(), 1, 10))
                              .ThrowsAsync(new ArgumentOutOfRangeException());

            var filter = new EventsFilter();

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => _eventService.GetFiltratedEventsWithPagingAsync(filter));
        }

        [Fact]
        public async Task GetFiltratedEventsWithPagingAsync_EmptyQuery_ReturnsEmptyPaginationResult()
        {
            // Arrange
            _mockEventsRepository.Setup(r => r.GetAllAsQueryable())
                                 .Returns(new List<Event>().AsQueryable());

            var paginatedResult = new PaginationResult<Event>
            {
                Items = new List<Event>(),
                PageNumber = 1,
                TotalAmount = 0
            };

            _mockPaginationService.Setup(p => p.ApplyPagingAsync<Event>(It.IsAny<IQueryable<Event>>(), 1, 10))
                              .ReturnsAsync(paginatedResult);

            var filter = new EventsFilter();

            // Act
            var result = await _eventService.GetFiltratedEventsWithPagingAsync(filter);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result.Items);
            Assert.Equal(1, result.PageNumber);
            Assert.Equal(0, result.TotalAmount);
        }
    }
}
