using Bookstore.Domain.ReferenceData;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace Bookstore.Web.Areas.Admin.Models.ReferenceData
{
    public class ReferenceDataItemCreateUpdateViewModel
    {
        public ReferenceDataItemCreateUpdateViewModel() { }

        public ReferenceDataItemCreateUpdateViewModel(ReferenceDataItem referenceDataItem)
        {
            Id = referenceDataItem.Id;
            SelectedReferenceDataType = referenceDataItem.DataType;
            Text = referenceDataItem.Text;
        }

        public int Id { get; set; }

        public ReferenceDataType SelectedReferenceDataType { get; set; }

        public string Text { get; set; }

        public IEnumerable<SelectListItem> DataTypes { get; set; }
    }
}
