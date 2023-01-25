using Bookstore.Domain.ReferenceData;
using Bookstore.Web.Areas.Admin.Models.ReferenceData;

namespace Bookstore.Web.Areas.Admin.Mappers.ReferenceData
{
    public static class ReferenceDataItemToReferenceDataCreateUpdateViewModel
    {
        public static ReferenceDataItemCreateUpdateViewModel ToReferenceDataItemCreateUpdateViewModel(this ReferenceDataItem referenceDataItem)
        {
            var result = new ReferenceDataItemCreateUpdateViewModel
            {
                Id = referenceDataItem.Id,
                SelectedReferenceDataType = referenceDataItem.DataType,
                Text = referenceDataItem.Text
            };

            return result;
        }
    }
}
