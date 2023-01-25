using Bookstore.Domain.ReferenceData;
using Bookstore.Web.Areas.Admin.Models.ReferenceData;

namespace Bookstore.Web.Areas.Admin.Mappers.ReferenceData
{
    public static class ReferenceDataItemCreateUpdateViewModelToReferenceDataItemMapper
    {
        public static ReferenceDataItem ToReferenceDataItem(this ReferenceDataItemCreateUpdateViewModel viewModel)
        {
            var referenceDataItem = new ReferenceDataItem();

            return viewModel.ToReferenceDataItem(referenceDataItem);
        }

        public static ReferenceDataItem ToReferenceDataItem(this ReferenceDataItemCreateUpdateViewModel viewModel, ReferenceDataItem referenceDataItem)
        {
            referenceDataItem.DataType = viewModel.SelectedReferenceDataType;
            referenceDataItem.Text = viewModel.Text;

            return referenceDataItem;
        }
    }
}
