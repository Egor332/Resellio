using Microsoft.EntityFrameworkCore;

namespace ResellioBackend
{
    public class ResellioDbContext : DbContext
    {
        public ResellioDbContext(DbContextOptions<ResellioDbContext> options) : base(options) { }
    }
}
