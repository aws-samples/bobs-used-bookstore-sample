using Bookstore.Domain.ReferenceData;
using Bookstore.Web.ViewModel.Resale;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace Bookstore.Web.Mappers
{
    public static class ReferenceDataToResaleCreateViewModelMapper
    {
        public static void PopulateReferenceData(this ResaleCreateViewModel model, IEnumerable<ReferenceDataItem> referenceData)
        {
            model.BookTypes = referenceData.Where(x => x.DataType == ReferenceDataType.BookType).Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Text });
            model.Publishers = referenceData.Where(x => x.DataType == ReferenceDataType.Publisher).Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Text });
            model.Genres = referenceData.Where(x => x.DataType == ReferenceDataType.Genre).Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Text });
            model.Conditions = referenceData.Where(x => x.DataType == ReferenceDataType.Condition).Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Text });
        }
    }
}
