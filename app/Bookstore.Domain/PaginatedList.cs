namespace Bookstore.Domain
{
    public class PaginatedList<T> : List<T>
    {
        public int PageIndex { get; private set; }

        public int TotalPages { get; private set; }

        public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
        {
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

        public static PaginatedList<T> Create(IQueryable<T> source, int pageIndex, int pageSize)
        {
            var count = source.Count();
            var items = source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

            return new PaginatedList<T>(items, count, pageIndex, pageSize);
        }
    }
}
