using Bookstore.Admin.ViewModel.ReferenceData;
using Bookstore.Domain.ReferenceData;

namespace Bookstore.Admin.Mappers.ReferenceData
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
