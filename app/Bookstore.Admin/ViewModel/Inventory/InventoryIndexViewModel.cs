using Bookstore.Domain;
using Bookstore.Domain.Books;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdminSite.ViewModel.Inventory
{
    public class InventoryIndexViewModel
    {
        private const int MaximumButtonCount = 5;

        public InventoryIndexViewModel(PaginatedList<Book> books)
        {
            foreach (var book in books)
            {
                Items.Add(new InventoryIndexListItemViewModel
                {
                    Id = book.Id,
                    Name = book.Name,
                    Author = book.Author,
                    BookType = book.BookType.Text,
                    Condition = book.Condition?.Text,
                    Genre = book.Genre.Text,
                    Price = 0,
                    Publisher = book.Publisher.Text,
                    UpdatedOn = book.UpdatedOn,
                    Year = book.Year.GetValueOrDefault()
                });
            }

            PageIndex = books.PageIndex;
            PageSize = books.Count;
            PageCount = books.TotalPages;
            HasNextPage = books.HasNextPage;
            HasPreviousPage = books.HasPreviousPage;
            PaginationButtons = books.GetPageList(5).ToList();
        }

        public List<InventoryIndexListItemViewModel> Items { get; set; } = new List<InventoryIndexListItemViewModel>();

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public int PageCount { get; set; }

        public bool HasNextPage { get; set; }

        public bool HasPreviousPage { get; set; }

        public List<int> PaginationButtons { get; set; } = new List<int>();
    }

    public class InventoryIndexListItemViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Author { get; set; }

        public int Year { get; set; }

        public string Publisher { get; set; }

        public string Genre { get; set; }

        public string BookType { get; set; }

        public string Condition { get; set; }

        public decimal Price { get; set; }

        public DateTime UpdatedOn { get; set; }
    }
}
