
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace ResellioBackend.Common.Paging
{
    public class PaginationService : IPaginationService
    {
        public async Task<PaginationResult<T>> ApplyPagingAsync<T>(IQueryable<T> query, int page, int pageSize)
        {           

            if (!query.Expression.ToString().Contains("OrderBy"))
            {
                throw new InvalidOperationException("You must apply OrderBy before paging.");
            }

            if (page < 1)
            {
                throw new ArgumentOutOfRangeException($"Page number must be higher than 0, got {page}");
            }

            if (pageSize < 1) 
            {
                throw new ArgumentOutOfRangeException($"Page size number must be higher than 0, got {pageSize}");
            }
            int totalAmount;
            List<T> items;
            if (query.Provider is IAsyncQueryProvider)
            {
                totalAmount = await query.CountAsync();
                items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            }
            else
            {
                totalAmount = query.Count();
                items = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            }

            return new PaginationResult<T>
            {
                Items = items,
                TotalAmount = totalAmount,
                PageNumber = page,
            };
        }
    }
}
