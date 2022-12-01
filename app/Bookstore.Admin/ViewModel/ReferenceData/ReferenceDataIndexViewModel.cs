using Bookstore.Admin.ViewModel;
using System.Collections.Generic;

namespace AdminSite.ViewModel.ReferenceData
{
    public class ReferenceDataIndexViewModel : PaginatedViewModel
    {
        public List<ReferenceDataIndexListItemViewModel> Items { get; set; } = new List<ReferenceDataIndexListItemViewModel>();
    }

    public class ReferenceDataIndexListItemViewModel
    {
        public int Id { get; set; }

        public string Text { get; set; }

        public string ReferenceDataType { get; set; }
    }
}
