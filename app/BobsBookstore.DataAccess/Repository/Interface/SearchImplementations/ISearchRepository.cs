using System.Linq.Expressions;

namespace BobsBookstore.DataAccess.Repository.Interface.SearchImplementations
{
    public interface ISearchRepository
    {
        BinaryExpression ReturnExpression(ParameterExpression parameterExpression, string filterValue,
            string searchString);

        int GetTotalPages(int totalCount, int valsPerPage);

        int[] GetModifiedPagesArr(int pageNum, int totalPages);
    }
}