using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BookstoreBackend.Repository.SearchImplementations
{
    public interface ISearchRepository
    {


        BinaryExpression ReturnExpression(ParameterExpression parameterExpression, string filterValue, string searchString);

        int GetTotalPages(int totalCount, int valsPerPage);

        int[] GetModifiedPagesArr(int pageNum, int totalPages);
    }
}
