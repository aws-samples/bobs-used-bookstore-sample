using Bookstore.Domain.ReferenceData;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace Bookstore.Admin.ViewModel.ReferenceData
{
    public class ReferenceDataItemCreateUpdateViewModel
    {
        public int Id { get; set; }

        public ReferenceDataType SelectedReferenceDataType { get; set; }

        public string Text { get; set; }

        public IEnumerable<SelectListItem> DataTypes { get; set; }
    }
}
