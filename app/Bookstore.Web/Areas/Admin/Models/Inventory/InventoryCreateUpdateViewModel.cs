using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Bookstore.Domain.Books;
using Bookstore.Domain.ReferenceData;
using Bookstore.Web.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Bookstore.Web.Areas.Admin.Models.Inventory
{
    public class InventoryCreateUpdateViewModel
    {
        public InventoryCreateUpdateViewModel() { }

        public InventoryCreateUpdateViewModel(IEnumerable<ReferenceDataItem> referenceDataItems)
        {
            AddReferenceData(referenceDataItems);
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

        [Required]
        public string Name { get; set; }

        [Required]
        public string Author { get; set; }

        public int Year { get; set; }

        [Required]
        public string ISBN { get; set; }

        public IEnumerable<SelectListItem> Publishers { get; set; } = new List<SelectListItem>();
        
        [Required]
        [DisplayName("Publisher")]
        public int SelectedPublisherId { get; set; }

        public IEnumerable<SelectListItem> BookTypes { get; set; } = new List<SelectListItem>();

        [Required]
        [DisplayName("Book Type")]
        public int SelectedBookTypeId { get; set; }

        public IEnumerable<SelectListItem> Genres { get; set; } = new List<SelectListItem>();
       
        [Required]
        [DisplayName("Genre")]
        public int SelectedGenreId { get; set; }

        public IEnumerable<SelectListItem> BookConditions { get; set; } = new List<SelectListItem>();
        
        [Required]
        [DisplayName("Condition")]
        public int SelectedConditionId { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public int Quantity { get; set; } = 1;

        [MaxFileSize(2 * 1024 * 1024)]
        [ImageTypes(new[] { ".png", ".jpg", ".jpeg" })]
        [DisplayName("Cover image")]
        public IFormFile CoverImage { get; set; }
        
        public string CoverImageUrl { get; set; }

        public string Summary { get; set; }

        public void AddReferenceData(IEnumerable<ReferenceDataItem> referenceDataItems)
        {
            var dataItems = referenceDataItems.ToList();
            BookConditions = dataItems
                .Where(x => x.DataType == ReferenceDataType.Condition)
                .Select(x => new SelectListItem(x.Text, x.Id.ToString()));

            BookTypes = dataItems
                .Where(x => x.DataType == ReferenceDataType.BookType)
                .Select(x => new SelectListItem(x.Text, x.Id.ToString()));

            Genres = dataItems
                .Where(x => x.DataType == ReferenceDataType.Genre)
                .Select(x => new SelectListItem(x.Text, x.Id.ToString()));

            Publishers = dataItems
                .Where(x => x.DataType == ReferenceDataType.Publisher)
                .Select(x => new SelectListItem(x.Text, x.Id.ToString()));
        }
    }
}