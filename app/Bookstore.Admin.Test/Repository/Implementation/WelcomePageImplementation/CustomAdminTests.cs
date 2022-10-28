
using System;
using Xunit;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using AdminSite.Test.Repository;
using DataAccess.Repository.Implementation.WelcomePageImplementation;
using AdminSite.ViewModel.UpdateBooks;
using DataModels.Orders;
using DataModels.Customers;
using DataAccess.Dtos;
using Bookstore.Domain.Orders;
using Bookstore.Domain.Books;

namespace Bobs_Backend.Test
{
    public class CustomAdminTests
    {
        private static List<Price> GetSampleTestData()
        {
            var sampleData = new List<Price>
            {
                new Price
                    {
                        Price_Id = 29, Book = new Book(), Condition = new Condition(), ItemPrice = 154, Quantity = 30, UpdatedBy = "admin",
                        UpdatedOn = DateTime.ParseExact("2020-07-31", "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture), Active = false, RowVersion = new byte[] { 0x20, 0x20 }
                    },
                new Price
                    {
                        Price_Id = 30, Book = new Book(), Condition = new Condition(), ItemPrice = 154, Quantity = 3, UpdatedBy = "admin",
                        UpdatedOn = DateTime.ParseExact("2020-07-31", "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture), Active = false, RowVersion = new byte[] { 0x20, 0x20 }
                    },
                new Price
                   {
                       Price_Id = 31, Book = new Book(), Condition = new Condition(), ItemPrice = 140, Quantity = 13, UpdatedBy = "admin",
                       UpdatedOn = DateTime.ParseExact("2020-08-01", "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture), Active = false, RowVersion = new byte[] { 0x20, 0x20 }
                   }
            };

            return sampleData;
        }

        [Fact]
        
        public async void GetUserUpdateBooks_ShouldReturnUserUpdatedBooks_WhenUserExists()
        {
            //Arrange
            var connect = new MockDatabaseRepo();
            
            var context1 = connect.CreateInMemoryContext();
            
            const string adminUsername = "admin";
            var sampleData = GetSampleTestData();
            foreach(var data in sampleData)
            {
                context1.Price.Add(data);
            }
            await context1.SaveChangesAsync();
            //Act
            var _sut = new CustomAdmin(context1);
            var UserBooks = await _sut.GetUserUpdatedBooksAsync(adminUsername);

            //Assert
            Assert.NotNull(UserBooks);
            foreach (var bookInstance in UserBooks)
            {
                Assert.Equal(adminUsername, bookInstance.UpdatedBy);
            }

            await context1.DisposeAsync();
        }
  
        [Fact]
        public async void GetUserUpdateBooks_ShouldReturnZeroValues_WhenUserHasNoUpdates()
        {
            var connect = new MockDatabaseRepo();
            var context = connect.CreateInMemoryContext();
            const string adminUsername = "admin2";
            var sampleData = GetSampleTestData();
            foreach(var data in sampleData)
            {
                context.Price.Add(data);
            }
            var _sut = new CustomAdmin(context);
            //Act
            var result = await _sut.GetUserUpdatedBooksAsync(adminUsername);
            
            Assert.True(result.Count == 0);
            await context.DisposeAsync();
        }

        [Fact]
        public async Task GetUserUpdateBooks_ShouldReturnException_WhenAdminUserisNull()
        {
            //Arrange
            var connect = new MockDatabaseRepo();
            var context = connect.CreateInMemoryContext();
            string adminUsername = null ;
            var sampleData = GetSampleTestData();
            foreach(var data in sampleData)
            {
                context.Price.Add(data);
            }

            //Act
            var _sut = new CustomAdmin(context);
            //Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _sut.GetUserUpdatedBooksAsync(adminUsername));
            await context.DisposeAsync();
        }



        [Fact]
        public async void OtherUpdatedBooks_ShouldReturnBooksByOtherUsers_WhenGivenUser()
        { 
            var connect = new MockDatabaseRepo();
            var context1 = connect.CreateInMemoryContext();
            const string adminUsername = "admin";
            context1.Price.Add(
                    new Price
                    {
                        Price_Id = 29, Book = new Book(), Condition = new Condition(), ItemPrice = 154, Quantity = 30, UpdatedBy = "admin2",
                        UpdatedOn = DateTime.ParseExact("2020-07-31", "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture), Active = false, RowVersion = new byte[] { 0x20, 0x20 }
                    }
               
               );
            await context1.SaveChangesAsync();
            //Act
            var _sut = new CustomAdmin(context1);
            var otherBooks = await _sut.OtherUpdatedBooksAsync(adminUsername);

            Assert.NotNull(otherBooks);
            Assert.True(otherBooks.Count == 1);
            await context1.DisposeAsync();
        }

        [Fact]
        public async void OtherUpdatedBooks_ShouldReturnZeroValues_NoOtherUserExists()
        {
            var connect = new MockDatabaseRepo();
            var context1 = connect.CreateInMemoryContext();
            const string adminUsername = "admin";
            var sampleData = GetSampleTestData();
            foreach(var data in sampleData)
            {
                context1.Price.Add(data);
            }
            var _sut = new CustomAdmin(context1);
            var otherBooks = await _sut.OtherUpdatedBooksAsync(adminUsername);
            Assert.Empty(otherBooks);
        }

        [Fact]
        public async void OtherUpdateBooks_ShouldReturnException_WhenUserIsNull()
        {
            var connect = new MockDatabaseRepo();
            var context = connect.CreateInMemoryContext();
            string adminUsername = null;
            var sampleData = GetSampleTestData();
            foreach(var data in sampleData)
            {
                context.Price.Add(data);
            }
            var _sut = new CustomAdmin(context);
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await _sut.OtherUpdatedBooksAsync(adminUsername));
        }

        [Fact]
        public void GetOrderSeverity_ShouldReturnSeverity_WhenInputsNotNull()
        {
            //Arrange
            var connect = new MockDatabaseRepo();
            var context = connect.CreateInMemoryContext();
            var order =
                new Order
                {
                    Order_Id = 22,
                    Subtotal = 222,
                    Tax = 21,
                    DeliveryDate = "2020-08-15",
                    OrderStatus = new OrderStatus { OrderStatus_Id = 2, Status = "Pending", Position = 2 },
                    Customer = new Customer(),
                    Address = new Address(),

                };

            //Act
            var _sut = new CustomAdmin(context);
            var actualResult = _sut.GetOrderSeverity(order, 2);

            Assert.Equal(1, actualResult);
        }

        [Fact]
        
        public async void GetOrderSeverity_ShouldReturnException_WhenInputsAreNull()
        {
            var connect = new MockDatabaseRepo();
            var context = connect.CreateInMemoryContext();

            const int timeDiff = 2;
          
            var _sut = new CustomAdmin(context);

            var result = Assert.Throws<NullReferenceException>(() => _sut.GetOrderSeverity(/* Order */ null, timeDiff));
            Assert.Equal("Object reference not set to an instance of an object.", result.Message);
            await context.DisposeAsync();
        }

        [Fact]
        public async void GetImportantOrders_ShouldReturnFilteredOrders_GivenDateRange()
        {
            //Arrange
            var connect = new MockDatabaseRepo();
            var context = connect.CreateInMemoryContext();
            var expectedResult = new List<Order>()
            {
                new Order
                {
                    Order_Id = 42,
                    Subtotal = 222,
                    Tax = 21,
                    DeliveryDate = DateTime.Today.AddDays(-1).ToString("dd/MM/yyyy"),
                    OrderStatus = context.OrderStatus.Find((long) 2),
                    Customer = new Customer
                    {
                        Customer_Id = "123",
                        FirstName = "AB",
                        LastName = "CD",
                        DateOfBirth = DateTime.Parse("1996-08-01"),
                        Username = "ABCD",
                        Email = "abcd@gmail.com",
                        Phone = "12345678"
                    },
                    Address = new Address(),
                },
                new Order
                {
                    Order_Id = 43,
                    Subtotal = 2,
                    Tax = 1,
                    DeliveryDate = DateTime.Today.AddDays(1).ToString("dd/MM/yyyy"),
                    OrderStatus = context.OrderStatus.Find((long) 3),
                    Customer = new Customer
                    {
                        Customer_Id = "1234",
                        FirstName = "AB",
                        LastName = "CD",
                        DateOfBirth = DateTime.Parse("1996-08-01"),
                        Username = "ABCD",
                        Email = "abcd@gmail.com",
                        Phone = "12345678"
                    },
                    Address = new Address(),
                }
            };

            foreach (var order in expectedResult)
            {
                context.Order.Add(order);
            }

            await context.SaveChangesAsync();
            const int minDateRange = 0;
            const int maxDateRange = 4;

            //Act
            var _sut = new CustomAdmin(context);
            var actualResult = await _sut.GetImportantOrdersAsync(maxDateRange , minDateRange);
            
            //Assert
            Assert.NotNull(actualResult);
            Assert.Equal(2, actualResult.Count);
            for(var i=0; i < expectedResult.Count; i++)
            {
                Assert.Equal(expectedResult[i].Order_Id, actualResult[i].Order.Order_Id);
            }

            await context.DisposeAsync();
        }

        [Fact]
        public async void SortTable_ShouldSortOrderByPrice_WhenSortingByPrice()
        {
            var connect = new MockDatabaseRepo();
            var context = connect.CreateInMemoryContext();
            var filterOrders = new List<FilterOrdersDto>
            {
               new FilterOrdersDto
               {
                   Order = new Order
                   {
                        Order_Id = 22,
                        Subtotal = 22,
                        Tax = 21,
                        DeliveryDate = "2020-08-15",
                        OrderStatus = await context.OrderStatus.FindAsync((long) 2),
                        Customer = new Customer
                        {
                            Customer_Id ="123",
                            FirstName="AB",
                            LastName="CD",
                            DateOfBirth=DateTime.Parse("1996-08-01"),
                            Username="ABCD",
                            Email="abcd@gmail.com",
                            Phone="12345678"
                        },
                        Address = new Address(),
                   },
                   Severity = 2
               },
               new FilterOrdersDto
               {
                   Order = new Order
                   {
                        Order_Id = 23,
                        Subtotal = 222,
                        Tax = 1,
                        DeliveryDate = "2020-08-18",
                        OrderStatus = await context.OrderStatus.FindAsync((long) 3),
                        Customer = new Customer
                        {
                            Customer_Id ="1234",
                            FirstName="AB",
                            LastName="CD",
                            DateOfBirth=DateTime.Parse("1996-08-01"),
                            Username="ABCD",
                            Email="abcd@gmail.com",
                            Phone="12345678"
                        },
                        Address = new Address(),
                   },
                   Severity=1
               }
            };

            const string sortByValue = "price_desc";

            var _sut = new CustomAdmin(context);
            var result = _sut.SortTable(filterOrders, sortByValue);

            result = _sut.SortTable(filterOrders, sortByValue);

            Assert.NotNull(result);
            Assert.True(result[0].Order.Subtotal > result[1].Order.Subtotal);
        }

        [Fact]
        public void SortTable_ShouldSortOrderByPrice_WhenSortingByDate()
        {
            var connect = new MockDatabaseRepo();
            var context = connect.CreateInMemoryContext();
            var filterOrders = new List<FilterOrdersDto>
            {
               new FilterOrdersDto
               {
                   Order = new Order
                   {
                        Order_Id = 22,
                        Subtotal = 222,
                        Tax = 21,
                        DeliveryDate = "2020-08-15",
                        OrderStatus = new OrderStatus { OrderStatus_Id = 3, Status = "En Route", Position = 3 },
                        Customer = new Customer
                        {
                            Customer_Id ="123",
                            FirstName="AB",
                            LastName="CD",
                            DateOfBirth=DateTime.Parse("1996-08-01"),
                            Username="ABCD",
                            Email="abcd@gmail.com",
                            Phone="12345678"
                        },
                        Address = new Address(),
                   },
                   Severity = 2
               },
               new FilterOrdersDto
               {
                   Order = new Order
                   {
                        Order_Id = 23,
                        Subtotal = 2,
                        Tax = 1,
                        DeliveryDate = "2020-08-18",
                        OrderStatus = new OrderStatus { OrderStatus_Id = 2, Status = "Pending", Position = 2 },
                        Customer = new Customer
                        {
                            Customer_Id ="1234",
                            FirstName="AB",
                            LastName="CD",
                            DateOfBirth=DateTime.Parse("1996-08-01"),
                            Username="ABCD",
                            Email="abcd@gmail.com",
                            Phone="12345678"
                        },
                        Address = new Address(),
                   },
                   Severity=1
               }
            };

            var sortByValue = "date";

            var _sut = new CustomAdmin(context);
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
            var connect = new MockDatabaseRepo();
            var context = connect.CreateInMemoryContext();
            var filterOrders = new List<FilterOrdersDto>
            {
               new FilterOrdersDto
               {
                   Order = new Order
                   {
                        Order_Id = 22,
                        Subtotal = 222,
                        Tax = 21,
                        DeliveryDate = "2020-08-15",
                        OrderStatus = new OrderStatus { OrderStatus_Id = 3, Status = "En Route", Position = 3 },
                        Customer = new Customer
                        {
                            Customer_Id ="123",
                            FirstName="AB",
                            LastName="CD",
                            DateOfBirth=DateTime.Parse("1996-08-01"),
                            Username="ABCD",
                            Email="abcd@gmail.com",
                            Phone="12345678"
                        },
                        Address = new Address(),
                   },
                   Severity = 2
               },
               new FilterOrdersDto
               {
                   Order = new Order
                   {
                        Order_Id = 23,
                        Subtotal = 2,
                        Tax = 1,
                        DeliveryDate = "2020-08-18",
                        OrderStatus = new OrderStatus { OrderStatus_Id = 2, Status = "Pending", Position = 2 },
                        Customer = new Customer
                        {
                            Customer_Id ="1234",
                            FirstName="AB",
                            LastName="CD",
                            DateOfBirth=DateTime.Parse("1996-08-01"),
                            Username="ABCD",
                            Email="abcd@gmail.com",
                            Phone="12345678"
                        },
                        Address = new Address(),
                   },
                   Severity=1
               }
            };

            var sortByValue = "status";

            var _sut = new CustomAdmin(context);
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
