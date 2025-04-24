
using Microsoft.EntityFrameworkCore;

namespace ResellioBackend.Common.Paging
{
    public class PagingService : IPagingService
    {
        public async Task<PagingResult<T>> ApplyPagingAsync<T>(IQueryable<T> query, int page, int pageSize)
        {
            if (!query.Expression.ToString().Contains("OrderBy"))
            {
                throw new InvalidOperationException("You must apply OrderBy before paging.");
            }

            var totalAmount = await query.CountAsync();
            var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PagingResult<T>
            {
                Items = items,
                TotalAmount = totalAmount,
                PageNumber = page,
            };
        }
    }
}
