using BOBS_Backend.Models.Book;
using BOBS_Backend.Models.Order;
using BOBS_Backend.Repository.Implementations.SearchImplementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Xunit;

namespace Bobs_Backend.Test.Repository.Implementation.SearchImplementation
{
    public class SearchRepositoryTest
    {

        private SearchRepository _searchRepo;


        //[Fact]
        //public void GetModifiedPagesArr_WhenPageNumIsNotWithinTotalPageNum()
        //{
        //    MockDatabaseRepo connect = new MockDatabaseRepo();
        //    var context = connect.CreateInMemoryContext();

        //    _searchRepo = new SearchRepository(context);
        //    var pages = _searchRepo.GetModifiedPagesArr(4, 15);
            
        //    Assert.NotNull(pages);
        //    Assert.Equal("10", pages.Count() + "");
        //    Assert.Equal("1", pages[0] + "");
        //    Assert.Equal("10", pages[9] + "");
        //}

        //[Fact]
        //public void GetModifiedPagesArr_WhenPageNumIsNotWithinTotalPageNum_AndPageNumIsMultipleOf10()
        //{
        //    MockDatabaseRepo connect = new MockDatabaseRepo();
        //    var context = connect.CreateInMemoryContext();

        //    _searchRepo = new SearchRepository(context);
        //    var pages = _searchRepo.GetModifiedPagesArr(10, 15);

        //    Assert.NotNull(pages);
        //    Assert.Equal("10", pages.Count() + "");
        //    Assert.Equal("1", pages[0] + "");
        //    Assert.Equal("10", pages[9] + "");
        //}

        //[Fact]
        //public void GetModifiedPagesArr_WhenPageNumIsWithinTotalPageNum()
        //{
        //    MockDatabaseRepo connect = new MockDatabaseRepo();
        //    var context = connect.CreateInMemoryContext();

        //    _searchRepo = new SearchRepository(context);
        //    var pages = _searchRepo.GetModifiedPagesArr(11, 16);

        //    Assert.NotNull(pages);
        //    Assert.Equal("6", pages.Count() + "");
        //    Assert.Equal("11", pages[0] + "");
        //    Assert.Equal("16", pages[5] + "");
        //}
    
        //[Fact]
        //public void GetTotalPages_WhenNoReminder()
        //{
        //    MockDatabaseRepo connect = new MockDatabaseRepo();
        //    var context = connect.CreateInMemoryContext();

        //    _searchRepo = new SearchRepository(context);

        //    var totalPageNum = _searchRepo.GetTotalPages(131, 20);

        //    Assert.Equal("7", totalPageNum + "");

        //}

        //[Fact]
        //public void GetTotalPages_WhenIsReminder()
        //{
        //    MockDatabaseRepo connect = new MockDatabaseRepo();
        //    var context = connect.CreateInMemoryContext();

        //    _searchRepo = new SearchRepository(context);

        //    var totalPageNum = _searchRepo.GetTotalPages(140, 20);

        //    Assert.Equal("7", totalPageNum + "");
        //}

        //[Fact]
        //public void GetBaseQuey()
        //{
        //    MockDatabaseRepo connect = new MockDatabaseRepo();
        //    var context = connect.CreateInMemoryContext();

        //    _searchRepo = new SearchRepository(context);

        //    var orderQuery = (IQueryable<Order>) _searchRepo.GetBaseQuery("BOBS_Backend.Models.Order.Order");
        //    Assert.NotNull(orderQuery);

        //    var priceQuery = (IQueryable<Price>)_searchRepo.GetBaseQuery("BOBS_Backend.Models.Book.Price");
        //    Assert.NotNull(priceQuery);
        //}

        //[Fact]
        //public void ReturnExpression_OnlyMainTable_OneCategory_NumStringOne()
        //{
        //    MockDatabaseRepo connect = new MockDatabaseRepo();
        //    var context = connect.CreateInMemoryContext();

        //    _searchRepo = new SearchRepository(context);

        //    var parameterExpression = Expression.Parameter(typeof(Order),"order");


        //    var filterValue = " Order_Id";
        //    var searchString = "588";
        //    var expression = _searchRepo.ReturnExpression(parameterExpression, filterValue, searchString);
        //    var testString = expression.ToString();

        //    Assert.NotNull(expression);
        //    Assert.Equal("(order.Order_Id == 588)", testString);
        
        //}

        //[Fact]
        //public void ReturnExpression_OnlyMainTable_OneCategory_NumStringMultiple()
        //{
        //    MockDatabaseRepo connect = new MockDatabaseRepo();
        //    var context = connect.CreateInMemoryContext();

        //    _searchRepo = new SearchRepository(context);

        //    var parameterExpression = Expression.Parameter(typeof(Order), "order");


        //    var filterValue = " Order_Id";
        //    var searchString = "588 587";
        //    var expression = _searchRepo.ReturnExpression(parameterExpression, filterValue, searchString);
        //    var testString = expression.ToString();

        //    Assert.NotNull(expression);
        //    Assert.Equal("((order.Order_Id == 588) Or (order.Order_Id == 587))", testString);
        //}

        //[Fact]
        //public void ReturnExpression_OnlyMainTable_MultipleCategory_NumStringOne()
        //{
        //    MockDatabaseRepo connect = new MockDatabaseRepo();
        //    var context = connect.CreateInMemoryContext();

        //    _searchRepo = new SearchRepository(context);

        //    var parameterExpression = Expression.Parameter(typeof(Order), "order");


        //    var filterValue = " Order_Id DeliveryDate";
        //    var searchString = "588";
        //    var expression = _searchRepo.ReturnExpression(parameterExpression, filterValue, searchString);
        //    var testString = expression.ToString();

        //    Assert.NotNull(expression);
        //    Assert.Equal("((order.Order_Id == 588) And (order.DeliveryDate.Contains(\"588\") Or order.DeliveryDate.Contains(\"588\")))", testString);
        //}

        //[Fact]
        //public void ReturnExpression_OnlyMainTable_MultipleCategory_NumStringMultiple()
        //{
        //    MockDatabaseRepo connect = new MockDatabaseRepo();
        //    var context = connect.CreateInMemoryContext();

        //    _searchRepo = new SearchRepository(context);

        //    var parameterExpression = Expression.Parameter(typeof(Order), "order");


        //    var filterValue = " Order_Id DeliveryDate";
        //    var searchString = "588 587";
        //    var expression = _searchRepo.ReturnExpression(parameterExpression, filterValue, searchString);
        //    var testString = expression.ToString();

        //    Assert.NotNull(expression);
        //    Assert.Equal("(((order.Order_Id == 588) Or (order.Order_Id == 587)) And ((order.DeliveryDate.Contains(\"588\") Or order.DeliveryDate.Contains(\"588\")) Or (order.DeliveryDate.Contains(\"587\") Or order.DeliveryDate.Contains(\"587\"))))", testString);
        //}
    }
}
