using BobCustomerSite.Controllers;
using DataAccess.Repository.Interface;
using DataAccess.Repository.Interface.SearchImplementations;
using CustomerSite.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using Bookstore.Domain.Books;
using Bookstore.Domain.Carts;

namespace CustomerSite.Test.Controllers
{

    public class SearchControllerTest
    {
        private readonly Mock<IGenericRepository<CartItem>> mockCartItemRepository = new Mock<IGenericRepository<CartItem>>();
        private readonly Mock<IGenericRepository<Cart>> mockCartRepository = new Mock<IGenericRepository<Cart>>();
        private readonly Mock<IGenericRepository<Book>> mockBookRepository = new Mock<IGenericRepository<Book>>();
        private readonly Mock<IBookSearch> mockBookSearch = new Mock<IBookSearch>();
        private readonly Mock<IGenericRepository<Condition>> mockConditionRepository = new Mock<IGenericRepository<Condition>>();
        private readonly Mock<IGenericRepository<Price>> mockpriceRepository = new Mock<IGenericRepository<Price>>();
        private readonly Mock<IPriceSearch> mockPriceSearch = new Mock<IPriceSearch>();
       
        [Fact]
        public async void TestIndexView()
        {
        
            var controller = new SearchController(mockBookSearch.Object, mockPriceSearch.Object, mockConditionRepository.Object, mockCartItemRepository.Object, mockCartRepository.Object, mockpriceRepository.Object, mockBookRepository.Object);
            
            var result = await controller.Index("price_aesc","Potter", 1);

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<PaginationModel>(
        viewResult.ViewData.Model);

        }
    }
}
