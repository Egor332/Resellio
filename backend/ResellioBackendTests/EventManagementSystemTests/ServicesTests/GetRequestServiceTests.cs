using Moq;
using ResellioBackend.Common.Paging;
using ResellioBackend.EventManagementSystem.Filtering;
using ResellioBackend.EventManagementSystem.Repositories.Abstractions;
using ResellioBackend.EventManagementSystem.Services.Implementations;
using ResellioBackend.EventManagementSystem.Models;
using ResellioBackend.Common.Filters;

namespace ResellioBackendTests.EventManagementSystemTests.ServicesTests
{
    public class GetRequestServiceTests
    {
        private readonly Mock<IPaginationService> _mockPaginationService;
        private readonly GetRequestService _service;

        public GetRequestServiceTests()
        {
            _mockPaginationService = new Mock<IPaginationService>();
            _service = new GetRequestService(_mockPaginationService.Object);
        }

        [Fact]
        public async Task ApplyFiltersAndPaginationAsync_WhenOk_ShouldApplyFiltersAndReturnMappedDtos()
        {
            // Arrange
            var models = new List<TestModel>
            {
                new TestModel { Id = 1, Name = "Item1" },
                new TestModel { Id = 2, Name = "Item2" }
            }.AsQueryable();

            var filteredModels = models.Where(m => m.Id > 0);

            var filterMock = new Mock<IFiltrable<TestModel>>();
            filterMock.Setup(f => f.ApplyFilters(It.IsAny<IQueryable<TestModel>>()))
                .Returns(filteredModels);

            var paginatedModels = new PaginationResult<TestModel>
            {
                Items = filteredModels.ToList(),
                PageNumber = 1,
                TotalAmount = 2
            };

            _mockPaginationService.Setup(p => p.ApplyPaginationAsync<TestModel>(It.IsAny<IQueryable<TestModel>>(), 1, 10))
                .ReturnsAsync(paginatedModels);

            Func<TestModel, TestDto> mapper = model => new TestDto { Id = model.Id, Name = model.Name };

            // Act
            var result = await _service.ApplyFiltersAndPaginationAsync(models, mapper, filterMock.Object);

            // Assert
            Assert.Equal(2, result.Items.Count);
            Assert.Equal(1, result.PageNumber);
            Assert.Equal(2, result.TotalAmount);
            Assert.Collection(result.Items,
                item =>
                {
                    Assert.Equal(1, item.Id);
                    Assert.Equal("Item1", item.Name);
                },
                item =>
                {
                    Assert.Equal(2, item.Id);
                    Assert.Equal("Item2", item.Name);
                });

            filterMock.Verify(f => f.ApplyFilters(It.IsAny<IQueryable<TestModel>>()), Times.Once);
            _mockPaginationService.Verify(p => p.ApplyPaginationAsync<TestModel>(It.IsAny<IQueryable<TestModel>>(), 1, 10), Times.Once);
        }

        [Fact]
        public async Task ApplyFiltersAndPaginationAsync_WhenPageOutOfRange_ShouldThrow()
        {
            // Arrange
            var models = new List<TestModel>().AsQueryable();

            var filterMock = new Mock<IFiltrable<TestModel>>();
            filterMock.Setup(f => f.ApplyFilters(It.IsAny<IQueryable<TestModel>>()))
                .Returns(models);

            _mockPaginationService.Setup(p => p.ApplyPaginationAsync<TestModel>(It.IsAny<IQueryable<TestModel>>(), 100, 10))
                .ThrowsAsync(new ArgumentOutOfRangeException());

            Func<TestModel, TestDto> mapper = model => new TestDto { Id = model.Id, Name = model.Name };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
                _service.ApplyFiltersAndPaginationAsync(models, mapper, filterMock.Object, 100, 10));
        }
    }


    public class TestModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class TestDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
