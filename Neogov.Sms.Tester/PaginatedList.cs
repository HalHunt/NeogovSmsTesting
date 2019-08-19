using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Neogov.Sms.Tester
{
    public class PaginatedList<T> : List<T>
    {
        public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);

            AddRange(items);
        }

        public int PageIndex { get; }

        public int TotalPages { get; }

        public bool HasPreviousPage { get { return (PageIndex > 1); } }

        public bool HasNextPage { get { return (PageIndex < TotalPages); } }
    }

    public static class PaginatedListExtensions
    {
        public async static Task<PaginatedList<T>> ToPaginatedList<T>(this IQueryable<T> items, int pageIndex, int pageSize)
        {
            return new PaginatedList<T>(await items.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync(),
                await items.CountAsync(),
                pageIndex,
                pageSize);
        }
    }
}
