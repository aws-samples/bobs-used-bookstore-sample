using Bookstore.Domain.Books;
using System.Collections.Generic;
using System.Linq;

namespace Bookstore.Web.ViewModel.Home
{
    public class HomeIndexViewModel
    {
        public List<HomeIndexItemViewModel> Books { get; set; } = new List<HomeIndexItemViewModel>();

        public HomeIndexViewModel(IEnumerable<Book> books)
        {
            if (books == null) return;

            Books = books.Select(x => new HomeIndexItemViewModel
            {
                BookId = x.Id,
                CoverImageUrl = x.CoverImageUrl,
                BookPrice = x.Price,
                BookName = x.Name,
                HasLowStockLevels = x.IsLowInStock,
                IsOutOfStock = !x.IsInStock
            }).ToList();
        }
    }

    public class HomeIndexItemViewModel
    {
        public int BookId { get; set; }

        public string BookName { get; set; }

        public decimal BookPrice { get; set; }

        public string CoverImageUrl { get; set; }

        public bool HasLowStockLevels { get; set; }

        public bool IsOutOfStock { get; set; }
    }
}