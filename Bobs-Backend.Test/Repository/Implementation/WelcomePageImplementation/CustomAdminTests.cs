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

namespace Bobs_Backend.Test
{
    public class CustomAdminTests
    {
        private readonly CustomAdmin _sut;
        private readonly Mock<DatabaseContext> _mockDatabaseContext = new Mock<DatabaseContext>();

        public CustomAdminTests() {

            _sut = new CustomAdmin(_mockDatabaseContext.Object);

        }


        [Fact]
        public async Task GetUserUpdateBooks_ShouldReturnUserUpdatedBooks_WhenUserExists() 
        {
            //Arrange
            string adminUsername = "admin";
            var priceDto = new List<Price>
            {
               
            }
            _mockDatabaseContext.Setup(x => x.Price).Returns(priceDto);
            //Act
            var UserBooks = _sut.GetUserUpdatedBooks(adminUsername);


            //Assert
            Assert.Equal(adminUsername, UserBooks.Result[0].UpdatedBy);

        }
    }
}
