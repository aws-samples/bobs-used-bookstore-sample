using Bookstore.Domain.Books;

namespace Bookstore.Domain.Tests
{
    public class BookTests
    {
        [Fact]
        public void IsInStock_ReturnsTrue_When_QuantityIsGreaterThanZero()
        {
            var book = new Book("Test", "Test", "1234567890123", 1, 1, 1, 1, 1, 1);
            Assert.True(book.IsInStock);

            book = new Book("Test", "Test", "1234567890123", 1, 1, 1, 1, 1, 0);
            Assert.False(book.IsInStock);
        }

        [Fact]
        public void IsLowInStock_ReturnsTrue_When_QuantityIsLessThanOrEqualToThreshold()
        {
            var threshold = Book.LowBookThreshold;

            var book = new Book("Test", "Test", "1234567890123", 1, 1, 1, 1, 1, threshold - 1);
            Assert.True(book.IsLowInStock);

            book = new Book("Test", "Test", "1234567890123", 1, 1, 1, 1, 1, threshold);
            Assert.True(book.IsLowInStock);

            book = new Book("Test", "Test", "1234567890123", 1, 1, 1, 1, 1, threshold + 1);
            Assert.False(book.IsLowInStock);
        }

        [Fact]
        public void ReduceStockLevel_ReducesQuantityBySpecifiedAmount_When_Executed()
        {
            var book = new Book("Test", "Test", "1234567890123", 1, 1, 1, 1, 1, 100);
            var amountToReduce = 50;
            var expectedQuantity = book.Quantity - amountToReduce;

            book.ReduceStockLevel(amountToReduce);

            Assert.Equal(expectedQuantity, book.Quantity);
        }

        [Fact]
        public void ReduceStockLevel_DoesNotReduceQuantityBelowZero_When_Executed()
        {
            var book = new Book("Test", "Test", "1234567890123", 1, 1, 1, 1, 1, 50);
            var amountToReduce = 60;

            book.ReduceStockLevel(amountToReduce);

            Assert.True(book.Quantity == 0);
        }
    }
}