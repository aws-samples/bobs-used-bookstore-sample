using Amazon.Extensions.CognitoAuthentication;
using BobBookstoreFrontend.Controllers;
using BobsBookstore.DataAccess.Data;
using BobsBookstore.DataAccess.Repository.Interface;
using BobsBookstore.DataAccess.Repository.Interface.SearchImplementations;
using BobsBookstore.Models.Books;
using BobsBookstore.Models.Carts;
using BobsBookstore.Models.Customers;
using BookstoreFrontend.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookstoreFrontend.Test.Controllers
{
   
    public class SearchControllerTest
    {

        [Fact]
        public async void TestIndexView()
        {
            var mockCartItemRepository = new Mock<IGenericRepository<CartItem>>();
            var mockCartRepository = new Mock<IGenericRepository<Cart>>();
            var mockBookRepository = new Mock<IGenericRepository<Book>>();
            var mockBookSearch = new Mock<IBookSearch>();
            var mockConditionRepository = new Mock<IGenericRepository<Condition>>();
            var mockpriceRepository = new Mock<IGenericRepository<Price>>();
            var mockPriceSearch = new Mock<IPriceSearch>();
          
            var controller = new SearchController(mockBookSearch.Object, mockPriceSearch.Object, mockConditionRepository.Object, mockCartItemRepository.Object, mockCartRepository.Object, mockpriceRepository.Object, mockBookRepository.Object);
            
            var result = await controller.Index("price_aesc","Potter", 1);

            var viewResult = Assert.IsType<ViewResult>(result);

        }
    }
}
