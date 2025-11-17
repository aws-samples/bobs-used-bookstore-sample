using Bookstore.Domain;
using Bookstore.Domain.Books;
using Bookstore.Domain.ReferenceData;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bookstore.Web.Areas.Admin.Models.Inventory
{
    public class InventoryIndexViewModel : PaginatedViewModel
    {
        public List<InventoryIndexListItemViewModel> Items { get; set; } = new List<InventoryIndexListItemViewModel>();

        public BookFilters Filters { get; set; } = new BookFilters();

        public IEnumerable<SelectListItem> Publishers { get; set; } = new List<SelectListItem>();

        public IEnumerable<SelectListItem> BookTypes { get; set; } = new List<SelectListItem>();

        public IEnumerable<SelectListItem> Genres { get; set; } = new List<SelectListItem>();

        public IEnumerable<SelectListItem> BookConditions { get; set; } = new List<SelectListItem>();

        public InventoryIndexViewModel() { }

        public InventoryIndexViewModel(IPaginatedList<Book> books, IEnumerable<ReferenceDataItem> referenceDataItems)
        {
            foreach (var book in books)
            {
                Items.Add(new InventoryIndexListItemViewModel
                {
                    Id = book.Id,
                    Name = book.Name,
                    Author = book.Author,
                    BookType = book.BookType.Text,
                    Condition = book.Condition.Text,
                    Genre = book.Genre.Text,
                    Publisher = book.Publisher.Text,
                    UpdatedOn = book.UpdatedOn,
                    Year = book.Year.GetValueOrDefault(),
                    Price = book.Price,
                    Quantity = book.Quantity
                });
            }

            PageIndex = books.PageIndex;
            PageSize = books.Count;
            PageCount = books.TotalPages;
            HasNextPage = books.HasNextPage;
            HasPreviousPage = books.HasPreviousPage;
            PaginationButtons = books.GetPageList(5).ToList();

            var dataItems = referenceDataItems.ToList();
            BookConditions = dataItems.Where(x => x.DataType == ReferenceDataType.Condition).Select(x => new SelectListItem(x.Text, x.Id.ToString()));
            BookTypes = dataItems.Where(x => x.DataType == ReferenceDataType.BookType).Select(x => new SelectListItem(x.Text, x.Id.ToString()));
            Genres = dataItems.Where(x => x.DataType == ReferenceDataType.Genre).Select(x => new SelectListItem(x.Text, x.Id.ToString()));
            Publishers = dataItems.Where(x => x.DataType == ReferenceDataType.Publisher).Select(x => new SelectListItem(x.Text, x.Id.ToString()));
        }
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

        public int Quantity { get; set; }

        public DateTime UpdatedOn { get; set; }
    }
}