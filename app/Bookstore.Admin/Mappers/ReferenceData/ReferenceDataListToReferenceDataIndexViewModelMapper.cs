using Bookstore.Admin.ViewModel.ReferenceData;
using Bookstore.Domain.ReferenceData;
using System.Collections.Generic;
using System.Linq;

namespace Bookstore.Admin.Mappers.ReferenceData
{
    public static class ReferenceDataListToReferenceDataIndexViewModelMapper
    {
        public static List<ReferenceDataIndexListItemViewModel> ToReferenceDataIndexListItemViewModels(this IEnumerable<ReferenceDataItem> referenceDataItems)
        {
            var result = new List<ReferenceDataIndexListItemViewModel>();

            foreach (var item in referenceDataItems.OrderBy(x => x.DataType.ToString()))
            {
                result.Add(new ReferenceDataIndexListItemViewModel
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
