namespace Bookstore.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Bookstore.Domain;

    using Microsoft.EntityFrameworkCore;

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
            var count = await this.source.CountAsync();
            var items = await this.source.Skip((this.pageIndex - 1) * this.pageSize).Take(this.pageSize).ToListAsync();

            this.PageIndex = this.pageIndex;

            this.TotalPages = (int)Math.Ceiling(count / (double)this.pageSize);

            this.AddRange(items);
        }

        public bool HasPreviousPage => this.PageIndex > 1;

        public bool HasNextPage => this.PageIndex < this.TotalPages;

        //TODO Consider pulling this out into its own class, e.g. PaginationButtonGenerator
        public IEnumerable<int> GetPageList(int count)
        {
            //https://jithilmt.medium.com/logic-of-building-a-pagination-ui-component-a-thought-process-f057ee2d487e

            var pagesCount = 1;
            var newPagesCount = 1;
            var start = this.PageIndex;
            var end = this.PageIndex;

            while (pagesCount < count)
            {
                if (end + 1 <= this.TotalPages)
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
                PageIndex = this.PageIndex,
                TotalPages = this.TotalPages,
            };

            foreach (var item in this)
            {
                result.Add(convert(item));
            }

            return result;
        }
    }
}
