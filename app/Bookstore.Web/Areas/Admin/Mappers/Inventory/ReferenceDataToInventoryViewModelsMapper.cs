using Bookstore.Domain.ReferenceData;
using Bookstore.Web.Areas.Admin.Models.Inventory;
using Bookstore.Web.Areas.Admin.Models.Offers;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace Bookstore.Web.Areas.Admin.Mappers.Inventory
{
    public static class ReferenceDataToInventoryViewModelsMapper
    {
        public static InventoryCreateUpdateViewModel PopulateReferenceData(this InventoryCreateUpdateViewModel viewModel, IEnumerable<ReferenceDataItem> referenceData)
        {
            viewModel.BookConditions = referenceData.Where(x => x.DataType == ReferenceDataType.Condition).Select(x => new SelectListItem(x.Text, x.Id.ToString()));
            viewModel.BookTypes = referenceData.Where(x => x.DataType == ReferenceDataType.BookType).Select(x => new SelectListItem(x.Text, x.Id.ToString()));
            viewModel.Genres = referenceData.Where(x => x.DataType == ReferenceDataType.Genre).Select(x => new SelectListItem(x.Text, x.Id.ToString()));
            viewModel.Publishers = referenceData.Where(x => x.DataType == ReferenceDataType.Publisher).Select(x => new SelectListItem(x.Text, x.Id.ToString()));

            return viewModel;
        }

        public static InventoryIndexViewModel PopulateReferenceData(this InventoryIndexViewModel viewModel, IEnumerable<ReferenceDataItem> referenceData)
        {
            viewModel.BookConditions = referenceData.Where(x => x.DataType == ReferenceDataType.Condition).Select(x => new SelectListItem(x.Text, x.Id.ToString()));
            viewModel.BookTypes = referenceData.Where(x => x.DataType == ReferenceDataType.BookType).Select(x => new SelectListItem(x.Text, x.Id.ToString()));
            viewModel.Genres = referenceData.Where(x => x.DataType == ReferenceDataType.Genre).Select(x => new SelectListItem(x.Text, x.Id.ToString()));
            viewModel.Publishers = referenceData.Where(x => x.DataType == ReferenceDataType.Publisher).Select(x => new SelectListItem(x.Text, x.Id.ToString()));

            return viewModel;
        }

        public static OfferIndexViewModel PopulateReferenceData(this OfferIndexViewModel viewModel, IEnumerable<ReferenceDataItem> referenceData)
        {
            viewModel.BookConditions = referenceData.Where(x => x.DataType == ReferenceDataType.Condition).Select(x => new SelectListItem(x.Text, x.Id.ToString()));
            viewModel.Genres = referenceData.Where(x => x.DataType == ReferenceDataType.Genre).Select(x => new SelectListItem(x.Text, x.Id.ToString()));

            return viewModel;
        }
    }
}