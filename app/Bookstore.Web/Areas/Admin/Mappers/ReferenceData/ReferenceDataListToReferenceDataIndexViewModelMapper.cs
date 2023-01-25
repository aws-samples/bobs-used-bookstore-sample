using Bookstore.Domain.ReferenceData;
using Bookstore.Web.Areas.Admin.Models.ReferenceData;
using System.Collections.Generic;
using System.Linq;

namespace Bookstore.Web.Areas.Admin.Mappers.ReferenceData
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
                    Id = item.Id,
                    ReferenceDataType = item.DataType.ToString(),
                    Text = item.Text
                });
            }

            return result;
        }
    }
}
