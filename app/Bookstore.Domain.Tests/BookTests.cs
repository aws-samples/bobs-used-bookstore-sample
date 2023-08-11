using Bookstore.Domain.Books;
using Bookstore.Domain.Tests.Builders;

namespace Bookstore.Domain.Tests
{
    public class BookTests
    {
        [Theory]
        [InlineData(1, true)]
        [InlineData(0, false)]
        public void IsInStock_ReturnsTrue_When_QuantityIsGreaterThanZero(int quantity, bool expectedResult)
        {
            var book = new BookBuilder()
                .Quantity(quantity)
                .Build();
            
            Assert.Equal(expectedResult, book.IsInStock);
        }

        [Theory]
        [InlineData(Book.LowBookThreshold - 1, true)]
        [InlineData(Book.LowBookThreshold, true)]
        [InlineData(Book.LowBookThreshold + 1, false)]
        public void IsLowInStock_ReturnsTrue_When_QuantityIsLessThanOrEqualToThreshold(int quantity, bool expectedResult)
        {
            var book = new BookBuilder()
                .Quantity(quantity)
                .Build();
            
            Assert.Equal(expectedResult, book.IsLowInStock);
        }

        [Fact]
        public void ReduceStockLevel_ReducesQuantityBySpecifiedAmount_When_Executed()
        {
            var book = new BookBuilder()
                .Quantity(100)
                .Build();
            
            const int amountToReduce = 50;
            var expectedQuantity = book.Quantity - amountToReduce;

            book.ReduceStockLevel(amountToReduce);

            Assert.Equal(expectedQuantity, book.Quantity);
        }

        [Fact]
        public void ReduceStockLevel_DoesNotReduceQuantityBelowZero_When_Executed()
        {
            var book = new BookBuilder()
                .Quantity(50)
                .Build();
            
            const int amountToReduce = 60;

            book.ReduceStockLevel(amountToReduce);

            Assert.Equal(0, book.Quantity);
        }
    }
}