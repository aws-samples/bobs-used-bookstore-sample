using Amazon.S3.Model;
using Autofac.Extras.Moq;
using BOBS_Backend.Controllers;
using BOBS_Backend.Models.Book;
using BOBS_Backend.Models.Customer;
using BOBS_Backend.Models.Order;
using BOBS_Backend.Repository.WelcomePageInterface;
using BOBS_Backend.ViewModel.UpdateBooks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Bobs_Backend.Test.Controller
{
    public class HomeControllerTests
    {

        public async Task<List<Price>> GetSampleValues()
        {
            var userBooks =new List<Price>
            {
                new Price
                {
                     Price_Id = 29,
                       Book = new Book(),
                       Condition = new Condition(),
                       ItemPrice = 154,
                       Quantity = 30,
                       UpdatedBy = "admin",
                       UpdatedOn = DateTime.ParseExact("2020-07-31", "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture),
                       Active = false,
                       RowVersion = new byte[] { 0x20, 0x20 }
                },
                new Price
                {
                     Price_Id = 30,
                       Book = new Book(),
                       Condition = new Condition(),
                       ItemPrice = 154,
                       Quantity = 30,
                       UpdatedBy = "admin",
                       UpdatedOn = DateTime.ParseExact("2020-07-30", "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture),
                       Active = false,
                       RowVersion = new byte[] { 0x20, 0x20 }
                }
            };
            return userBooks;
        }

        [Fact]
        public async Task WelcomePageTest_ShouldReturnViewWithModel_WhenSortByValueGivenAsync()
        {
            //Arrange

            var filterOrders = new List<FilterOrders>
            {
               new FilterOrders
               {
                   Order = new Order
                   {
                        Order_Id = 22,
                        Subtotal = 222,
                        Tax = 21,
                        DeliveryDate = "2020-08-15",
                        OrderStatus = new OrderStatus { OrderStatus_Id = 3, Status = "En Route", position = 3 },
                        Customer = new Customer{Customer_Id ="123" ,FirstName="AB", LastName="CD", DateOfBirth=DateTime.Parse("1996-08-01"), Username="ABCD", Email="abcd@gmail.com", Phone="12345678" },
                        Address = new Address(),
                        RowVersion = new byte[] { 0x20, 0x20 }
                   },
                   Severity = 2
               },
               new FilterOrders
               {
                   Order = new Order
                   {
                        Order_Id = 23,
                        Subtotal = 2,
                        Tax = 1,
                        DeliveryDate = "2020-08-18",
                        OrderStatus = new OrderStatus { OrderStatus_Id = 2, Status = "Pending", position = 2 },
                        Customer = new Customer{Customer_Id ="1234" ,FirstName="AB", LastName="CD", DateOfBirth=DateTime.Parse("1996-08-01"), Username="ABCD", Email="abcd@gmail.com", Phone="12345678" },
                        Address = new Address(),
                        RowVersion = new byte[] { 0x20, 0x20 }
                   },
                   Severity=1
               }
            };

            var user =GetSampleValues();
            string adminUsername = "admin";
            var notUserBooks = new List<Price>();
            var mock = AutoMock.GetLoose();
            mock.Mock<ICustomAdminPage>().Setup(c => c.GetImportantOrders(0, 5)).ReturnsAsync(filterOrders);
            mock.Mock<ICustomAdminPage>().Setup(c => c.GetUserUpdatedBooks(adminUsername)).Returns(user);
            mock.Mock<ICustomAdminPage>().Setup(c => c.OtherUpdatedBooks("admin")).ReturnsAsync(notUserBooks);
            var cls = mock.Create<HomeController>();
            
            
            //Act
            var result = cls.WelcomePage("price");
            
            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<LatestUpdates>>(viewResult.ViewData.Model);
            foreach(var mod in model)
            {
                Assert.NotNull(mod);
            }


        }

    }
}
