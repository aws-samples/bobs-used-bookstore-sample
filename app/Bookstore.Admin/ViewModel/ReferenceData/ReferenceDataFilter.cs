using Bookstore.Domain.ReferenceData;

namespace Bookstore.Admin.ViewModel.ReferenceData
{
    public class ReferenceDataFilter
    {
        public ReferenceDataType? ReferenceDataTypeFilter { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public ReferenceDataFilter()
        {
            PageIndex= 1;
            PageSize= 10;
        }
    }
}
