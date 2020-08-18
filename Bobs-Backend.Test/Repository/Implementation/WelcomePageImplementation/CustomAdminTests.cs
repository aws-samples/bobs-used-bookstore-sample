using System;
using Xunit;
using Bobs_Backend;
using BOBS_Backend.Repository.Implementations.WelcomePageImplementation;
using Moq;
using Microsoft.EntityFrameworkCore.Storage;
using BOBS_Backend.Database;
using System.Threading.Tasks;
using BOBS_Backend.Models.Book;
using System.Collections.Generic;
using System.Security.Claims;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Autofac.Extras.Moq;
using Bobs_Backend.Test.Repository;
using Microsoft.AspNetCore.Http;
using BOBS_Backend.Models.Order;
using BOBS_Backend.Models.Customer;
using System.Data.Entity.Core.Metadata.Edm;
using BOBS_Backend.ViewModel.UpdateBooks;

namespace Bobs_Backend.Test
{
    public class CustomAdminTests
    {
        private readonly CustomAdmin _sut;
        private readonly Mock<DatabaseContext> _mockDatabaseContext = new Mock<DatabaseContext>();

        
        [Fact]
        
        public  void GetUserUpdateBooks_ShouldReturnUserUpdatedBooks_WhenUserExists()
        {

            //Arrange
            MockDatabaseRepo connect = new MockDatabaseRepo();
            
           

            var context1 = connect.CreateInMemoryContext();
            
            string adminUsername = "admin";
            context1.Price.Add(
                    new Price
                    {
                        Price_Id = 29, Book = new Book(), Condition = new Condition(), ItemPrice = 154, Quantity = 30, UpdatedBy = "admin",
                        UpdatedOn = DateTime.ParseExact("2020-07-31", "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture), Active = false, RowVersion = new byte[] { 0x20, 0x20 }
                    }
                );
            context1.Price.Add(
                    new Price
                    {
                        Price_Id = 30, Book = new Book(), Condition = new Condition(), ItemPrice = 120, Quantity = 13, UpdatedBy = "admin",
                        UpdatedOn = DateTime.ParseExact("2020-08-01", "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture), Active = false, RowVersion = new byte[] { 0x20, 0x20 }
                    }
                );
            context1.Price.Add(
                   new Price
                   {
                       Price_Id = 31, Book = new Book(), Condition = new Condition(), ItemPrice = 140, Quantity = 13, UpdatedBy = "admin2",
                       UpdatedOn = DateTime.ParseExact("2020-08-01", "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture), Active = false, RowVersion = new byte[] { 0x20, 0x20 }
                   }
               );
            context1.SaveChanges();
            //Act
            CustomAdmin _sut = new CustomAdmin(context1);
            var UserBooks = _sut.GetUserUpdatedBooks(adminUsername);

            //Assert
            Assert.NotNull(UserBooks);
            foreach (var bookinstance in UserBooks.Result)
            {
                Assert.Equal(adminUsername, bookinstance.UpdatedBy);
            }

            context1.Dispose();
        }
  
        [Fact]
        public void  GetUserUpdateBooks_ShouldReturnZeroValues_WhenUserHasNoUpdates()
        {
            MockDatabaseRepo connect = new MockDatabaseRepo();
            var context = connect.CreateInMemoryContext();
            string adminUsername = "admin2";
             context.Price.Add(
                    new Price
                    {
                        Price_Id = 29, Book = new Book(), Condition = new Condition(), ItemPrice = 154, Quantity = 30, UpdatedBy = "admin",
                        UpdatedOn = DateTime.ParseExact("2020-07-31", "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture), Active = false, RowVersion = new byte[] { 0x20, 0x20 }
                    }
                );
            context.Price.Add(
                    new Price
                    {
                        Price_Id = 30, Book = new Book(), Condition = new Condition(), ItemPrice = 120, Quantity = 13, UpdatedBy = "admin",
                        UpdatedOn = DateTime.ParseExact("2020-08-01", "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture), Active = false, RowVersion = new byte[] { 0x20, 0x20 }
                    }
                );
            CustomAdmin _sut = new CustomAdmin(context);
            //Act
            var result = _sut.GetUserUpdatedBooks(adminUsername);
            
            Assert.True(result.Result.Count == 0);
            context.Dispose();
        }

        [Fact]
        public async Task GetUserUpdateBooks_ShouldReturnException_WhenAdminUserisNull()
        {
            
            MockDatabaseRepo connect = new MockDatabaseRepo();
            var context = connect.CreateInMemoryContext();
            string adminUsername = null ;
            context.Price.Add(
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
                   }
               );
            context.Price.Add(
                    new Price
                    {
                        Price_Id = 30,
                        Book = new Book(),
                        Condition = new Condition(),
                        ItemPrice = 120,
                        Quantity = 13,
                        UpdatedBy = "admin",
                        UpdatedOn = DateTime.ParseExact("2020-08-01", "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture),
                        Active = false,
                        RowVersion = new byte[] { 0x20, 0x20 }
                    }
                );

            
            CustomAdmin _sut = new CustomAdmin(context);
            await Assert.ThrowsAsync<ArgumentNullException>(() => _sut.GetUserUpdatedBooks(adminUsername));
        }



        [Fact]
        public void OtherUpdatedBooks_ShouldReturnBooksByOtherUsers_WhenGivenUser() 
        { 
            MockDatabaseRepo connect = new MockDatabaseRepo();
            var context1 = connect.CreateInMemoryContext();
            string adminUsername = "admin";
            context1.Price.Add(
                    new Price
                    {
                        Price_Id = 29, Book = new Book(), Condition = new Condition(), ItemPrice = 154, Quantity = 30, UpdatedBy = "admin",
                        UpdatedOn = DateTime.ParseExact("2020-07-31", "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture), Active = false, RowVersion = new byte[] { 0x20, 0x20 }
                    }
                );
            context1.Price.Add(
                    new Price
                    {
                        Price_Id = 30, Book = new Book(), Condition = new Condition(), ItemPrice = 120, Quantity = 13, UpdatedBy = "admin",
                        UpdatedOn = DateTime.ParseExact("2020-08-01", "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture), Active = false, RowVersion = new byte[] { 0x20, 0x20 }
                    }
                );
            context1.Price.Add(
                   new Price
                   {
                       Price_Id = 31, Book = new Book(), Condition = new Condition(), ItemPrice = 140, Quantity = 13, UpdatedBy = "admin2",
                       UpdatedOn = DateTime.ParseExact("2020-08-01", "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture), Active = false, RowVersion = new byte[] { 0x20, 0x20 }
                   }
               );
            context1.SaveChanges();
            //Act
            CustomAdmin _sut = new CustomAdmin(context1);
            var otherBooks = _sut.OtherUpdatedBooks(adminUsername);
            Assert.NotNull(otherBooks);
            Assert.True(otherBooks.Result.Count == 1);
            context1.Dispose();
                
        }

        [Fact]
        public void OtherUpdatedBooks_ShouldReturnZeroValues_NoOtherUserExists()
        {
            MockDatabaseRepo connect = new MockDatabaseRepo();
            var context1 = connect.CreateInMemoryContext();
            string adminUsername = "admin";
            context1.Price.Add(
                    new Price
                    {
                        Price_Id = 29, Book = new Book(), Condition = new Condition(), ItemPrice = 154, Quantity = 30, UpdatedBy = "admin",
                        UpdatedOn = DateTime.ParseExact("2020-07-31", "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture), Active = false, RowVersion = new byte[] { 0x20, 0x20 }
                    }
                );
            context1.Price.Add(
                    new Price
                    {
                        Price_Id = 30, Book = new Book(), Condition = new Condition(), ItemPrice = 120, Quantity = 13, UpdatedBy = "admin",
                        UpdatedOn = DateTime.ParseExact("2020-08-01", "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture), Active = false, RowVersion = new byte[] { 0x20, 0x20 }
                    }
                );
            CustomAdmin _sut = new CustomAdmin(context1);
            var otherBooks = _sut.OtherUpdatedBooks(adminUsername);
            Assert.Empty(otherBooks.Result);
        }

        [Fact]
        public void OtherUpdateBooks_ShouldReturnException_WhenUserIsNull()
        {
            MockDatabaseRepo connect = new MockDatabaseRepo();
            var context = connect.CreateInMemoryContext();
            string adminUsername = null;
            context.Price.Add(
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
                   }
               );
            context.Price.Add(
                    new Price
                    {
                        Price_Id = 30,
                        Book = new Book(),
                        Condition = new Condition(),
                        ItemPrice = 120,
                        Quantity = 13,
                        UpdatedBy = "admin",
                        UpdatedOn = DateTime.ParseExact("2020-08-01", "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture),
                        Active = false,
                        RowVersion = new byte[] { 0x20, 0x20 }
                    }
                );
            CustomAdmin _sut = new CustomAdmin(context);
            Assert.ThrowsAsync<ArgumentNullException>(() => _sut.OtherUpdatedBooks(adminUsername));
        }

        [Fact]
        public void GetOrderSeverity_ShouldReturnSeverity_WhenInputsNotNull()
        {
            //Arrange
            MockDatabaseRepo connect = new MockDatabaseRepo();
            var context = connect.CreateInMemoryContext();
           var order = 
                new Order
                {
                    Order_Id = 22,
                    Subtotal = 222,
                    Tax = 21,
                    DeliveryDate = "2020-08-15",
                    OrderStatus = new OrderStatus { OrderStatus_Id = 2, Status = "Pending", position = 2 },
                    Customer = new Customer(),
                    Address = new Address(),
                    RowVersion = new byte[] { 0x20, 0x20 }

                };

            //Act
            CustomAdmin _sut = new CustomAdmin(context);
            int actualResult = _sut.GetOrderSeverity(order, 2);

            //Assert
           
            Assert.Equal(1, actualResult);

        }

        [Fact]
        
        public void GetOrderSeverity_ShouldReturnException_WhenInputsAreNull()
        {
            MockDatabaseRepo connect = new MockDatabaseRepo();
            var context = connect.CreateInMemoryContext();
            var order = new Order();
            order = null;
            int timeDiff = 2;
          
            CustomAdmin _sut = new CustomAdmin(context);


            var result = Assert.Throws<NullReferenceException>(() => _sut.GetOrderSeverity(order, timeDiff));
            Assert.Equal("Object reference not set to an instance of an object.", result.Message);
            context.Dispose();

        }

        [Fact]
        public void GetImportantOrders_ShouldReturnFilteredOrders_GivenDateRange()
        {
            //Arrange
            MockDatabaseRepo connect = new MockDatabaseRepo();
            var context = connect.CreateInMemoryContext();
            var expectedResult = new List<Order> {

                new Order
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
                new Order
                {
                     Order_Id = 23,
                     Subtotal = 2,
                     Tax = 1,
                     DeliveryDate = "2020-08-18",
                     OrderStatus = new OrderStatus { OrderStatus_Id = 2, Status = "Pending", position = 2 },
                     Customer = new Customer{Customer_Id ="1234" ,FirstName="AB", LastName="CD", DateOfBirth=DateTime.Parse("1996-08-01"), Username="ABCD", Email="abcd@gmail.com", Phone="12345678" },
                     Address = new Address(),
                     RowVersion = new byte[] { 0x20, 0x20 }

                }
            };
            foreach(var order in expectedResult)
            {
                context.Order.Add(order);
            }
            context.SaveChanges();
            int minDateRange = 0;
            int maxDateRange = 4;
            //Act

            CustomAdmin _sut = new CustomAdmin(context);
            var actualResult = _sut.GetImportantOrders(maxDateRange , minDateRange).Result;
            
            //Assert
            Assert.NotNull(actualResult);
            Assert.Equal(2, actualResult.Count());
            for(int i=0;i<expectedResult.Count;i++)
            {
                Assert.Equal(expectedResult[i].Order_Id, actualResult[i].Order.Order_Id);
            }

            context.Dispose();

        }

        [Fact]
        public void SortTable_ShouldSortOrderByPrice_WhenSortingByPrice()
        {
            MockDatabaseRepo connect = new MockDatabaseRepo();
            var context = connect.CreateInMemoryContext();
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
            string sortByValue = "price";

            CustomAdmin _sut = new CustomAdmin(context);
            var result = _sut.SortTable(filterOrders, sortByValue);


            Assert.NotNull(result);
            Assert.True(result[0].Order.Subtotal < result[1].Order.Subtotal);

            sortByValue = "price_desc";
            result = _sut.SortTable(filterOrders, sortByValue);


            Assert.NotNull(result);
            Assert.True(result[0].Order.Subtotal > result[1].Order.Subtotal);


        }

        [Fact]
        public void SortTable_ShouldSortOrderByPrice_WhenSortingByDate()
        {
            MockDatabaseRepo connect = new MockDatabaseRepo();
            var context = connect.CreateInMemoryContext();
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
            string sortByValue = "date";

            CustomAdmin _sut = new CustomAdmin(context);
            var result = _sut.SortTable(filterOrders, sortByValue);


            Assert.NotNull(result);
            Assert.True(DateTime.Parse(result[0].Order.DeliveryDate) < DateTime.Parse(result[1].Order.DeliveryDate));

            sortByValue = "date_desc";
            result = _sut.SortTable(filterOrders, sortByValue);


            Assert.NotNull(result);
            Assert.True(DateTime.Parse(result[0].Order.DeliveryDate) > DateTime.Parse(result[1].Order.DeliveryDate));

        }

        [Fact]
        public void SortTable_ShouldSortOrderByPrice_WhenSortingByOrderStatus()
        {
            MockDatabaseRepo connect = new MockDatabaseRepo();
            var context = connect.CreateInMemoryContext();
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
            string sortByValue = "status";

            CustomAdmin _sut = new CustomAdmin(context);
            var result = _sut.SortTable(filterOrders, sortByValue);


            Assert.NotNull(result);
            Assert.Equal(-1,string.Compare(result[0].Order.OrderStatus.Status ,result[1].Order.OrderStatus.Status));

            sortByValue = "status_desc";
            result = _sut.SortTable(filterOrders, sortByValue);


            Assert.NotNull(result);
            Assert.Equal(1, string.Compare(result[0].Order.OrderStatus.Status, result[1].Order.OrderStatus.Status));

        }
    }
}
