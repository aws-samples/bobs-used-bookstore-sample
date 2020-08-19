using BOBS_Backend.Database;
using BOBS_Backend.Models.Order;
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
        private DatabaseContext _context;
        public ExpressionFunction(DatabaseContext context)
        {
            _context = context;
        }

        private BinaryExpression PerformArtithmeticExpresion(string operand, Expression property, ConstantExpression constant)
        {
            if (operand.Equals(">")) return Expression.GreaterThan(property, constant);
            if (operand.Equals("==")) return Expression.Equal(property, constant);
            if (operand.Equals("<")) return Expression.LessThan(property, constant);

            return Expression.Equal(property, constant);
        }

        //private BinaryExpression GenerateExpressionObject(string type, string subSearch, MemberExpression property, bool isEntire)
        //{
        //    try
        //    {

        //        var converter = TypeDescriptor.GetConverter(Type.GetType(type));

        //        var test = converter.ConvertFrom(subSearch);
        //        ConstantExpression constant = null;
        //        if (type == "System.Int64")
        //        {
        //            long value = 0;

        //            bool res = long.TryParse(subSearch, out value);

        //            constant = Expression.Constant(test);

        //            return PerformArtithmeticExpresion("==", (Expression)property, constant);
        //        }
        //        else
        //        {
        //            constant = Expression.Constant(subSearch);
        //            var method = typeof(string).GetMethod("Contains", new Type[] { typeof(string) });

        //            var expression = Expression.Call(property, method, constant);


        //            return Expression.Or(expression, expression);
        //        }

        //    }
        //    catch
        //    {
        //        return null;
        //    }

        //}

        //private BinaryExpression GenerateDynamicLambdaFunctionObjectProperty(string splitFilter, ParameterExpression parameterExpression, string searchString)
        //{
        //    var property = Expression.Property(parameterExpression, splitFilter);

        //    BinaryExpression lambda = null;
        //    bool isFirst = true;
        //    searchString = searchString.Trim();

        //    var table = (IQueryable)_context.GetType().GetProperty("Order").GetValue(_context, null);

        //    var row = Expression.Parameter(table.ElementType, "row");

        //    var col = Expression.Property(row, splitFilter);

        //    var type = col.Type.FullName;

        //    foreach (var subSearch in searchString.Split(' '))
        //    {
        //        try
        //        {

        //            var expression = GenerateExpressionObject(type, subSearch, property, false);


        //            if (isFirst)
        //            {
        //                lambda = expression;
        //                isFirst = false;


        //            }
        //            else
        //            {
        //                lambda = Expression.Or(lambda, expression);
        //                isFirst = false;
        //            }


        //        }
        //        catch
        //        {
        //        }
        //    }

        //    //var exp2 = GenerateExpressionOrder(type, searchString, method, property, true);

        //    //lambda = (isFirst == true) ? Expression.Or(exp2, exp2) : Expression.Or(lambda, exp2);

        //    return lambda;

        //}

        //private BinaryExpression GenerateExpressionSubObject(string type, string subSearch, string[] splitFilter, ParameterExpression parameterExpression, bool isEntire)
        //{
        //    try
        //    {
        //        ConstantExpression constant = null;
        //        if (type == "System.Int64")
        //        {
        //            long value = 0;

        //            bool res = long.TryParse(subSearch, out value);

        //            constant = Expression.Constant(value);

        //            Expression property2 = parameterExpression;

        //            foreach (var member in splitFilter)
        //            {
        //                property2 = Expression.PropertyOrField(property2, member);
        //            }

        //            var expression = PerformArtithmeticExpresion("==", property2, constant);
        //            return expression;
        //        }
        //        else
        //        {
        //            constant = Expression.Constant(subSearch);
        //            var method = typeof(string).GetMethod("Contains", new Type[] { typeof(string) });

        //            Expression property2 = parameterExpression;

        //            foreach (var member in splitFilter)
        //            {
        //                property2 = Expression.PropertyOrField(property2, member);
        //            }

        //            var expression = (isEntire == true) ? Expression.Call(constant, method, property2) : Expression.Call(property2, method, constant);

        //            return Expression.Or(expression, expression);
        //        }


        //    }
        //    catch
        //    {
        //        return null;
        //    }

        //}

        //private BinaryExpression GenerateDynamicLambdaFunctionSubObjectProperty(string[] splitFilter, ParameterExpression parameterExpression, string searchString)
        //{
        //    BinaryExpression lambda = null;

        //    bool isFirst = true;
        //    searchString = searchString.Trim();

        //    var table = (IQueryable)_context.GetType().GetProperty(splitFilter[0]).GetValue(_context, null);

        //    var row = Expression.Parameter(table.ElementType, "row");

        //    var col = Expression.Property(row, splitFilter[1]);

        //    var type = col.Type.FullName;
        //    foreach (var subSearch in searchString.Split(' '))
        //    {
        //        try
        //        {


        //            var expression = GenerateExpressionSubObject(type, subSearch, splitFilter, parameterExpression, false);

        //            if (isFirst)
        //            {

        //                lambda = expression;
        //                isFirst = false;


        //            }
        //            else
        //            {
        //                lambda = Expression.Or(lambda, expression);
        //                isFirst = false;
        //            }

        //        }
        //        catch
        //        {
        //        }
        //    }

        //    //var exp2 = GenerateExpressionSubOrder(type, searchString, method, splitFilter, parameterExpression, true);

        //    //lambda = (isFirst == true) ? Expression.Or(exp2, exp2) : Expression.Or(lambda, exp2); 
        //    return lambda;

        //}


        private UnaryExpression GenerateExpressionSubObject(ParameterExpression parameterExpression,string[] splitFilter, string type, string subSearch, string operand, string negate)
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

            UnaryExpression result = (negate == "true") ? Expression.Not(exp) : Expression.Not(Expression.Not(exp));

            return result;
        }
            
        private UnaryExpression GenerateDynamicLambdaFunctionSubObjectProperty(ParameterExpression parameterExpression, string[] splitFilter, string searchString, string operand, string negate)
        {
            var table = (IQueryable)_context.GetType().GetProperty(splitFilter[0]).GetValue(_context, null);

            var row = Expression.Parameter(table.ElementType, "row");

            var col = Expression.Property(row, splitFilter[1]);

            var type = col.Type.FullName;

            var result = GenerateExpressionSubObject(parameterExpression, splitFilter, type, searchString, operand, negate);

            return result;
        }

        
        private UnaryExpression GenerateExpressionObject(MemberExpression property, string type, string subSearch, string operand, string negate)
        {
            var converter = TypeDescriptor.GetConverter(Type.GetType(type));

            var value = converter.ConvertFrom(subSearch);
            ConstantExpression constant = null;

            constant = Expression.Constant(value);

            var exp = PerformArtithmeticExpresion(operand, property, constant);

            UnaryExpression result = (negate == "true") ? Expression.Not(exp) : Expression.Not(Expression.Not(exp));

            return result;
        }
        private UnaryExpression GenerateDynamicLambdaFunctionObjectProperty(ParameterExpression parameterExpression,string tableName,string filterCat,string searchString, string operand, string negate)
        {
            var property = Expression.Property(parameterExpression, filterCat);


            searchString = searchString.Trim();

            var table = (IQueryable)_context.GetType().GetProperty(tableName).GetValue(_context, null);

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

                UnaryExpression exp2 = null;

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
                    expression = Expression.Or(exp2,exp2);
                    isFirst = false;


                }
                else
                {
                    expression = (splitInBetweenVal[inBetweenCount] == "And") ? Expression.And(expression, exp2) : Expression.Or(expression, exp2);
                    inBetweenCount += 1;
                    isFirst = false;
                }



            }


            return expression;
        }
    }
}
