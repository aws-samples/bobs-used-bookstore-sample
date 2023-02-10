using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bookstore.Domain
{
    public class PaginatedList<T> : List<T>, IPaginatedList<T>
    {
        private readonly IQueryable<T> source;
        private readonly int pageIndex;
        private readonly int pageSize;

        public int PageIndex { get; private set; }

        public int TotalPages { get; private set; }

        private PaginatedList(){ }

        public PaginatedList(IQueryable<T> source, int pageIndex, int pageSize) 
        {
            this.source = source;
            this.pageIndex = pageIndex;
            this.pageSize = pageSize;
        }

        public async Task PopulateAsync()
        {
            var count = await source.CountAsync();
            var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();

            PageIndex = pageIndex;

            TotalPages = (int)Math.Ceiling(count / (double)pageSize);

            AddRange(items);
        }

        public bool HasPreviousPage => PageIndex > 1;

        public bool HasNextPage => PageIndex < TotalPages;

        //TODO Consider pulling this out into its own class, e.g. PaginationButtonGenerator
        public IEnumerable<int> GetPageList(int count)
        {
            //https://jithilmt.medium.com/logic-of-building-a-pagination-ui-component-a-thought-process-f057ee2d487e

            var pagesCount = 1;
            var newPagesCount = 1;
            var start = PageIndex;
            var end = PageIndex;

            while (pagesCount < count)
            {
                if (end + 1 <= TotalPages)
                {
                    end++;
                    newPagesCount++;
                }

                if (start - 1 > 0)
                {
                    start--;
                    newPagesCount++;
                }

                if (newPagesCount == pagesCount)
                {
                    break;
                }
                else
                {
                    pagesCount = newPagesCount;
                }
            }

            return Enumerable.Range(start, pagesCount);
        }

        public IPaginatedList<TConvertTo> ConvertTo<TConvertTo>(Func<T, TConvertTo> convert)
        {
            var result = new PaginatedList<TConvertTo>
            {
                PageIndex = PageIndex,
                TotalPages = TotalPages,
            };

            foreach (var item in this)
            {
                result.Add(convert(item));
            }

            return result;
        }
    }
}
