using Bookstore.Admin.ViewModel.ReferenceData;
using Bookstore.Domain.ReferenceData;

namespace Bookstore.Admin.Mappers.ReferenceData
{
    public static class ReferenceDataItemCreateUpdateViewModelToReferenceDataItemMapper
    {
        public static ReferenceDataItem ToReferenceDataItem(this ReferenceDataItemCreateUpdateViewModel viewModel)
        {
            var referenceDataItem = new ReferenceDataItem();

            return ToReferenceDataItem(viewModel, referenceDataItem);
        }

        public static ReferenceDataItem ToReferenceDataItem(this ReferenceDataItemCreateUpdateViewModel viewModel, ReferenceDataItem referenceDataItem)
        {
            referenceDataItem.DataType = viewModel.SelectedReferenceDataType;
            referenceDataItem.Text = viewModel.Text;

            return referenceDataItem;
        }
    }
}
