using Bookstore.Services.Filters;
using System.Collections.Generic;

namespace Bookstore.Admin.ViewModel.ReferenceData
{
    public class ReferenceDataIndexViewModel : PaginatedViewModel
    {
        public List<ReferenceDataIndexListItemViewModel> Items { get; set; } = new List<ReferenceDataIndexListItemViewModel>();

        public ReferenceDataFilters Filters { get; set; } = new ReferenceDataFilters();
    }

    public class ReferenceDataIndexListItemViewModel
    {
        public int Id { get; set; }

        public string Text { get; set; }

        public string ReferenceDataType { get; set; }
    }
}
