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
using BOBS_Backend;
using BOBS_Backend.Repository.Implementations;
using Amazon.Runtime.Internal.Util;
using BOBS_Backend.Repository.SearchImplementations;
using Microsoft.Extensions.Logging;

namespace Bobs_Backend.Test
{
    public class InventoryTests
    {
        private readonly Inventory _sut;
        private readonly Mock<DatabaseContext> _mockDatabaseContext = new Mock<DatabaseContext>();
        private readonly Mock<IInventory> _mockInventoryRepo = new Mock<IInventory>();
        private readonly Mock<IRekognitionNPollyRepository> _mockRekognitionRepo = new Mock<IRekognitionNPollyRepository>();
        private readonly Mock<Microsoft.Extensions.Logging.ILogger<Inventory>> _mockLogger = new Mock<Microsoft.Extensions.Logging.ILogger<Inventory>>();
        private readonly Mock<ISearchRepository> _mockSearch = new Mock<ISearchRepository>();


        [Fact]
        public void AutoSugggestions()
        {
            MockDatabaseRepo connect = new MockDatabaseRepo();
            var context = connect.CreateInMemoryContext();
            var Input = "ha";


            Inventory _sut = new Inventory(context, _mockSearch.Object, _mockRekognitionRepo.Object, _mockLogger.Object);
            var Suggestions = _sut.autosuggest(Input);
            Assert.NotNull(Suggestions);
            context.Dispose();
        }

    }
}