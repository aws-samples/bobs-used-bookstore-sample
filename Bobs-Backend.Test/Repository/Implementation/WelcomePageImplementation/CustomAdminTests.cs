
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



    }
}
