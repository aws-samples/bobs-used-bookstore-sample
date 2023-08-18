namespace Bookstore.Domain
{
    public interface IPaginatedList<T> : IList<T>
    {
        bool HasNextPage { get; }

        bool HasPreviousPage { get; }

        int PageIndex { get; }

        int TotalPages { get; }

        Task PopulateAsync();

        IEnumerable<int> GetPageList(int count);

        IPaginatedList<TConvertTo> ConvertTo<TConvertTo>(Func<T, TConvertTo> expression);
    }
}