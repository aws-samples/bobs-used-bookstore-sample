using Bookstore.Domain;
using Bookstore.Domain.ReferenceData;
using System.Collections.Generic;
using System.Linq;

namespace Bookstore.Web.Areas.Admin.Models.ReferenceData
{
    public class ReferenceDataIndexViewModel : PaginatedViewModel
    {
        public List<ReferenceDataIndexListItemViewModel> Items { get; set; } = new List<ReferenceDataIndexListItemViewModel>();

        public ReferenceDataFilters Filters { get; set; } = new ReferenceDataFilters();

        public ReferenceDataIndexViewModel(IPaginatedList<ReferenceDataItem> referenceDataItems, ReferenceDataFilters filters)
        {
            foreach (var item in referenceDataItems.OrderBy(x => x.DataType.ToString()))
            {
                Items.Add(new ReferenceDataIndexListItemViewModel
                {
                    Id = item.Id,
                    ReferenceDataType = item.DataType.ToString(),
                    Text = item.Text
                });
            }

            Filters = filters;

            PageIndex = referenceDataItems.PageIndex;
            PageSize = referenceDataItems.Count;
            PageCount = referenceDataItems.TotalPages;
            HasNextPage = referenceDataItems.HasNextPage;
            HasPreviousPage = referenceDataItems.HasPreviousPage;
            PaginationButtons = referenceDataItems.GetPageList(5).ToList();
        }
    }

    public class ReferenceDataIndexListItemViewModel
    {
        public int Id { get; set; }

        public string Text { get; set; }

        public string ReferenceDataType { get; set; }
    }
}
