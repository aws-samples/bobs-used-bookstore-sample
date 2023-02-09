using System.Collections.Generic;
using System.Linq;
using Bookstore.Domain.Books;
using Bookstore.Domain.ReferenceData;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Bookstore.Web.Areas.Admin.Models.Inventory
{
    public class InventoryCreateUpdateViewModel
    {
        public InventoryCreateUpdateViewModel() { }

        public InventoryCreateUpdateViewModel(IEnumerable<ReferenceDataItem> referenceDataItems)
        {
            BookConditions = referenceDataItems
                .Where(x => x.DataType == ReferenceDataType.Condition)
                .Select(x => new SelectListItem(x.Text, x.Id.ToString()));
            
            BookTypes = referenceDataItems
                .Where(x => x.DataType == ReferenceDataType.BookType)
                .Select(x => new SelectListItem(x.Text, x.Id.ToString()));
            
            Genres = referenceDataItems
                .Where(x => x.DataType == ReferenceDataType.Genre)
                .Select(x => new SelectListItem(x.Text, x.Id.ToString()));
            
            Publishers = referenceDataItems
                .Where(x => x.DataType == ReferenceDataType.Publisher)
                .Select(x => new SelectListItem(x.Text, x.Id.ToString()));
        }

        public InventoryCreateUpdateViewModel(IEnumerable<ReferenceDataItem> referenceDataItems, Book book) : this(referenceDataItems)
        {
            Author = book.Author;
            CoverImageUrl = book.CoverImageUrl;
            Id = book.Id;
            ISBN = book.ISBN;
            Name = book.Name;
            Price = book.Price;
            Quantity = book.Quantity;
            SelectedBookTypeId = book.BookTypeId;
            SelectedConditionId = book.ConditionId;
            SelectedGenreId = book.GenreId;
            SelectedPublisherId = book.PublisherId;
            Summary = book.Summary;
            Year = book.Year.GetValueOrDefault();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string Author { get; set; }

        public int Year { get; set; }

        public string ISBN { get; set; }

        public IEnumerable<SelectListItem> Publishers { get; set; } = new List<SelectListItem>();
        public int SelectedPublisherId { get; set; }

        public IEnumerable<SelectListItem> BookTypes { get; set; } = new List<SelectListItem>();
        public int SelectedBookTypeId { get; set; }

        public IEnumerable<SelectListItem> Genres { get; set; } = new List<SelectListItem>();
        public int SelectedGenreId { get; set; }

        public IEnumerable<SelectListItem> BookConditions { get; set; } = new List<SelectListItem>();
        public int SelectedConditionId { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; } = 1;

        public IFormFile CoverImage { get; set; }
        public string CoverImageUrl { get; set; }

        public string Summary { get; set; }
    }
}