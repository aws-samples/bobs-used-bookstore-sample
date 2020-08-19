using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BOBS_Backend.Repository.SearchImplementations
{
    public interface ISearchRepository
    {

        IQueryable GetBaseQuery(string objPath);

        BinaryExpression ReturnExpression(ParameterExpression parameterExpression, string filterValue, string searchString);

        int GetTotalPages(int totalCount, int valsPerPage);

        int[] GetModifiedPagesArr(int pageNum, int totalPages);
    }
}
