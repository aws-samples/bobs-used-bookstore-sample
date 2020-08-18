using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BOBS_Backend.Repository.SearchImplementations
{
    public interface IExpressionFunction
    {
        BinaryExpression ReturnExpression(string filterValue, string tableName, ParameterExpression parameterExpression, string searchString, string inBetween, string operand, string negate);
    }
}
