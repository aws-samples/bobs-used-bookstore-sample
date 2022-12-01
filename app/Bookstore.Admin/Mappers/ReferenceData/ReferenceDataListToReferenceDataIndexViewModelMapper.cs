using AdminSite.ViewModel.ReferenceData;
using Bookstore.Domain.ReferenceData;
using System.Collections.Generic;
using System.Linq;

namespace Bookstore.Admin.Mappers.ReferenceData
{
    public static class ReferenceDataListToReferenceDataIndexViewModelMapper
    {
        public static ReferenceDataIndexViewModel ToReferenceDataIndexViewModel(this IEnumerable<ReferenceDataItem> referenceDataItems)
        {
            var result = new ReferenceDataIndexViewModel();

            foreach (var item in referenceDataItems.OrderBy(x => x.DataType.ToString()))
            {
                result.Items.Add(new ReferenceDataIndexListItemViewModel
                {
                    Id= item.Id,
                    ReferenceDataType = item.DataType.ToString(),
                    Text= item.Text
                });
            }

            return result;
        }
    }
}
