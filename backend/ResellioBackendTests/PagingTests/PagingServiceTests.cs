using Microsoft.EntityFrameworkCore;
using ResellioBackend.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResellioBackendTests.PagingTests
{
    public class PagingServiceTests
    {
        private readonly PagingService _pagingService;

        public PagingServiceTests()
        {
            _pagingService = new PagingService();
        }

        private IQueryable<TestEntity> GetTestData(int count)
        {
            return Enumerable.Range(1, count)
                .Select(i => new TestEntity { Id = i, Name = $"Item {i}" })
                .AsQueryable();
        }

        [Fact]
        public async Task ApplyPagingAsync_WhenNoOrderBy_ThrowsException()
        {
            // Arrange
            var data = GetTestData(10);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _pagingService.ApplyPagingAsync(data, 1, 5));
        }

        private async Task<AppDbContext> CreateDbContextWithDataAsync(int count)
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var context = new AppDbContext(options);

            for (int i = 1; i <= count; i++)
            {
                context.TestEntities.Add(new TestEntity { Id = i, Name = $"Item {i}" });
            }

            await context.SaveChangesAsync();
            return context;
        }

        [Fact]
        public async Task ApplyPagingAsync_WhenCorrect_ReturnsCorrectPage()
        {
            var context = await CreateDbContextWithDataAsync(10);
            var query = context.TestEntities.OrderBy(e => e.Id);

            var result = await _pagingService.ApplyPagingAsync(query, page: 2, pageSize: 3);

            Assert.Equal(10, result.TotalAmount);
            Assert.Equal(2, result.PageNumber);
            Assert.Equal(3, result.Items.Count());
            Assert.Equal(4, result.Items.First().Id);
        }

        [Fact]
        public async Task ApplyPagingAsync_IfPageTooHigh_ReturnsEmptyList()
        {
            // Arrange
            var context = await CreateDbContextWithDataAsync(10);
            var query = context.TestEntities.OrderBy(e => e.Id);

            // Act
            var result = await _pagingService.ApplyPagingAsync(query, page: 3, pageSize: 5);

            // Assert
            Assert.Equal(10, result.TotalAmount);
            Assert.Equal(3, result.PageNumber);
            Assert.Empty(result.Items);
        }

        public class TestEntity
        {
            public int Id { get; set; }
            public string Name { get; set; } = default!;
        }

        public class AppDbContext : DbContext
        {
            public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

            public DbSet<TestEntity> TestEntities { get; set; }
        }
    }
}
