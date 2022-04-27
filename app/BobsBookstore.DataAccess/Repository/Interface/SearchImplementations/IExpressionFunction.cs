using System;
using System.Linq.Expressions;

namespace BobsBookstore.DataAccess.Repository.Interface.SearchImplementations
{
    public interface IExpressionFunction
    {
        BinaryExpression ReturnExpression(string filterValue, string tableName, ParameterExpression parameterExpression,
            string searchString, string inBetween, string operand, string negate);

        ParameterExpression ReturnParameterExpression(Type objType, string name);

        Expression<Func<T, bool>> ReturnLambdaExpression<T>(string tableName, string filterValue, string searchString,
            string inBetween, string operand, string negate);

        Expression<Func<T, bool>> ReturnLambdaExpression<T>(BinaryExpression expression,
            ParameterExpression parameterExpression);
    }
}