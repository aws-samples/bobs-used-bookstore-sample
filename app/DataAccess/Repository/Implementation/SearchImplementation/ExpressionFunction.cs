using System;
using System.ComponentModel;
using System.Linq.Expressions;
using DataAccess.Repository.Interface.SearchImplementations;

namespace DataAccess.Repository.Implementation.SearchImplementation
{
    public class ExpressionFunction : IExpressionFunction
    {
        private readonly ISearchDatabaseCalls _searchDbCalls;

        public ExpressionFunction(ISearchDatabaseCalls searchDatabaseCalls)
        {
            _searchDbCalls = searchDatabaseCalls;
        }

        public ParameterExpression ReturnParameterExpression(Type objType, string name)
        {
            var parameterExpression = Expression.Parameter(objType, name);

            return parameterExpression;
        }

        public BinaryExpression ReturnExpression(string filterValue, string tableName,
            ParameterExpression parameterExpression, string searchString, string inBetween, string operand,
            string negate)
        {
            var listOfFilters = filterValue.Split(' ');
            var isFirst = true;


            BinaryExpression expression = null;
            var inBetweenCount = 0;
            var splitInBetweenVal = inBetween.Split(' ');

            var splitSearchString = searchString.Split("&&");
            var splitOperand = operand.Split(' ');
            var splitNegate = negate.Split(' ');

            for (var i = 0; i < listOfFilters.Length; i++)
            {
                BinaryExpression exp2 = null;

                if (!listOfFilters[i].Contains("."))
                    exp2 = GenerateDynamicLambdaFunctionObjectProperty(parameterExpression, tableName, listOfFilters[i],
                        splitSearchString[i], splitOperand[i], splitNegate[i]);
                else
                    exp2 = GenerateDynamicLambdaFunctionSubObjectProperty(parameterExpression,
                        listOfFilters[i].Split("."), splitSearchString[i], splitOperand[i], splitNegate[i]);

                if (exp2 == null) continue;
                if (isFirst)
                {
                    if (splitNegate[i] == "false") expression = exp2;
                    else expression = Expression.Or(Expression.Not(exp2), Expression.Not(exp2));
                    isFirst = false;
                }
                else
                {
                    if (splitNegate[i] == "false")
                        expression = splitInBetweenVal[inBetweenCount] == "And"
                            ? Expression.And(expression, exp2)
                            : Expression.Or(expression, exp2);
                    else
                        expression = splitInBetweenVal[inBetweenCount] == "And"
                            ? Expression.And(expression, Expression.Not(exp2))
                            : Expression.Or(expression, Expression.Not(exp2));

                    inBetweenCount += 1;
                    isFirst = false;
                }
            }


            return expression;
        }

        public Expression<Func<T, bool>> ReturnLambdaExpression<T>(string tableName, string filterValue,
            string searchString, string inBetween, string operand, string negate)
        {
            var parameterExpression = ReturnParameterExpression(typeof(T), tableName);
            var expression = ReturnExpression(filterValue, tableName, parameterExpression, searchString, inBetween,
                operand, negate);

            return Expression.Lambda<Func<T, bool>>(expression, parameterExpression);
        }

        public Expression<Func<T, bool>> ReturnLambdaExpression<T>(BinaryExpression expression,
            ParameterExpression parameterExpression)
        {
            return Expression.Lambda<Func<T, bool>>(expression, parameterExpression);
        }

        private BinaryExpression PerformArtithmeticExpresion(string operand, Expression property,
            ConstantExpression constant)
        {
            if (operand.Equals(">")) return Expression.GreaterThan(property, constant);
            if (operand.Equals("==")) return Expression.Equal(property, constant);
            if (operand.Equals("<")) return Expression.LessThan(property, constant);
            if (operand.Equals(">=")) return Expression.GreaterThanOrEqual(property, constant);
            if (operand.Equals("<=")) return Expression.LessThanOrEqual(property, constant);
            return Expression.Equal(property, constant);
        }


        private BinaryExpression GenerateExpressionSubObject(ParameterExpression parameterExpression,
            string[] splitFilter, string type, string subSearch, string operand, string negate)
        {
            var converter = TypeDescriptor.GetConverter(Type.GetType(type));

            var value = converter.ConvertFrom(subSearch);
            ConstantExpression constant = null;

            constant = Expression.Constant(value);

            Expression property2 = parameterExpression;

            foreach (var member in splitFilter) property2 = Expression.PropertyOrField(property2, member);

            var exp = PerformArtithmeticExpresion(operand, property2, constant);

            return exp;
        }

        private BinaryExpression GenerateDynamicLambdaFunctionSubObjectProperty(ParameterExpression parameterExpression,
            string[] splitFilter, string searchString, string operand, string negate)
        {
            var table = _searchDbCalls.GetTable(splitFilter[0]);

            var row = Expression.Parameter(table.ElementType, "row");

            var col = Expression.Property(row, splitFilter[1]);

            var type = col.Type.FullName;

            var result =
                GenerateExpressionSubObject(parameterExpression, splitFilter, type, searchString, operand, negate);

            return result;
        }


        private BinaryExpression GenerateExpressionObject(MemberExpression property, string type, string subSearch,
            string operand, string negate)
        {
            var converter = TypeDescriptor.GetConverter(Type.GetType(type));

            var value = converter.ConvertFrom(subSearch);
            ConstantExpression constant = null;

            constant = Expression.Constant(value);

            var exp = PerformArtithmeticExpresion(operand, property, constant);

            return exp;
        }

        private BinaryExpression GenerateDynamicLambdaFunctionObjectProperty(ParameterExpression parameterExpression,
            string tableName, string filterCat, string searchString, string operand, string negate)
        {
            var property = Expression.Property(parameterExpression, filterCat);


            searchString = searchString.Trim();

            var table = _searchDbCalls.GetTable(tableName);

            var row = Expression.Parameter(table.ElementType, "row");

            var col = Expression.Property(row, filterCat);

            var type = col.Type.FullName;

            var result = GenerateExpressionObject(property, type, searchString, operand, negate);

            return result;
        }
    }
}