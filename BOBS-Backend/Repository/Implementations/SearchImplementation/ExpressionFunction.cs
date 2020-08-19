using BOBS_Backend.Database;
using BOBS_Backend.Models.Order;
using BOBS_Backend.Repository.OrdersInterface;
using BOBS_Backend.Repository.SearchImplementations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BOBS_Backend.Repository.Implementations.SearchImplementation
{
    public class ExpressionFunction : IExpressionFunction
    {
        private ISearchDatabaseCalls _searchDbCalls;

        public ExpressionFunction(ISearchDatabaseCalls searchDatabaseCalls)
        {
            _searchDbCalls = searchDatabaseCalls;
        }

        public ParameterExpression ReturnParameterExpression(Type objType, string name)
        {
            var parameterExpression = Expression.Parameter(objType, name);

            return parameterExpression;
        }

        private BinaryExpression PerformArtithmeticExpresion(string operand, Expression property, ConstantExpression constant)
        {
            if (operand.Equals(">")) return Expression.GreaterThan(property, constant);
            if (operand.Equals("==")) return Expression.Equal(property, constant);
            if (operand.Equals("<")) return Expression.LessThan(property, constant);
            if (operand.Equals(">=")) return Expression.GreaterThanOrEqual(property, constant);
            if (operand.Equals("<=")) return Expression.LessThanOrEqual(property, constant);
            return Expression.Equal(property, constant);
        }


        private BinaryExpression GenerateExpressionSubObject(ParameterExpression parameterExpression,string[] splitFilter, string type, string subSearch, string operand, string negate)
        {
            var converter = TypeDescriptor.GetConverter(Type.GetType(type));

            var value = converter.ConvertFrom(subSearch);
            ConstantExpression constant = null;

            constant = Expression.Constant(value);

            Expression property2 = parameterExpression;

            foreach (var member in splitFilter)
            {
                property2 = Expression.PropertyOrField(property2, member);
            }

            var exp = PerformArtithmeticExpresion(operand, property2, constant);

            return exp;
        }
            
        private BinaryExpression GenerateDynamicLambdaFunctionSubObjectProperty(ParameterExpression parameterExpression, string[] splitFilter, string searchString, string operand, string negate)
        {
            var table = _searchDbCalls.GetTable(splitFilter[0]);

            var row = Expression.Parameter(table.ElementType, "row");

            var col = Expression.Property(row, splitFilter[1]);

            var type = col.Type.FullName;

            var result = GenerateExpressionSubObject(parameterExpression, splitFilter, type, searchString, operand, negate);

            return result;
        }

        
        private BinaryExpression GenerateExpressionObject(MemberExpression property, string type, string subSearch, string operand, string negate)
        {
            var converter = TypeDescriptor.GetConverter(Type.GetType(type));

            var value = converter.ConvertFrom(subSearch);
            ConstantExpression constant = null;

            constant = Expression.Constant(value);

            var exp = PerformArtithmeticExpresion(operand, property, constant);

            return exp;
        }
        private BinaryExpression GenerateDynamicLambdaFunctionObjectProperty(ParameterExpression parameterExpression,string tableName,string filterCat,string searchString, string operand, string negate)
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

        public BinaryExpression ReturnExpression(string filterValue,string tableName, ParameterExpression parameterExpression,string searchString, string inBetween,string operand, string negate)
        {
            //string filterValue = "Order_Id Customer.Customer_Id";
            //string tableName = "Order";
            //var parameterExpression = Expression.Parameter(typeof(Order), "order");
            //var searchString = "47&&2";
            //var inBetween = "And";
            //var operand = "== ==";
            //var negate = "false true";

            string[] listOfFilters = filterValue.Split(' ');
            bool isFirst = true;


            
            BinaryExpression expression = null;
            int inBetweenCount = 0;
            string[] splitInBetweenVal = inBetween.Split(' ');

            string[] splitSearchString = searchString.Split("&&");
            string[] splitOperand = operand.Split(' ');
            string[] splitNegate = negate.Split(' ');

            for (int i = 0; i < listOfFilters.Length; i++)
            {

                BinaryExpression exp2 = null;

                if (!listOfFilters[i].Contains("."))
                {
                    exp2 = GenerateDynamicLambdaFunctionObjectProperty(parameterExpression, tableName, listOfFilters[i], splitSearchString[i],splitOperand[i],splitNegate[i]);
                }
                else
                {

                    //exp2 = GenerateDynamicLambdaFunctionSubObjectProperty(listOfFilters[i].Split("."), parameterExpression, searchString);
                    exp2 = GenerateDynamicLambdaFunctionSubObjectProperty(parameterExpression,listOfFilters[i].Split("."), splitSearchString[i], splitOperand[i], splitNegate[i]);


                }

                if (exp2 == null)
                {
                    continue;
                }
                if (isFirst)
                {
                    if (splitNegate[i] == "false") expression = exp2;
                    else expression = Expression.Or(Expression.Not(exp2), Expression.Not(exp2));
                    isFirst = false;


                }
                else
                {
                    if(splitNegate[i] == "false") expression = (splitInBetweenVal[inBetweenCount] == "And") ? Expression.And(expression, exp2) : Expression.Or(expression, exp2);
                    else expression = (splitInBetweenVal[inBetweenCount] == "And") ? Expression.And(expression, Expression.Not(exp2)) : Expression.Or(expression, Expression.Not(exp2));

                    inBetweenCount += 1;
                    isFirst = false;
                }



            }


            return expression;
        }

        public Expression<Func<T, bool>> ReturnLambdaExpression<T>(string tableName,string filterValue, string searchString, string inBetween, string operand, string negate)
        {
            var parameterExpression = ReturnParameterExpression(typeof(T), tableName);
            var expression = ReturnExpression(filterValue, tableName, parameterExpression, searchString, inBetween, operand, negate);

            return Expression.Lambda<Func<T, bool>>(expression, parameterExpression);
        }
    }
}
