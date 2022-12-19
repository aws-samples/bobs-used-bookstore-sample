using Bookstore.Data;
using Bookstore.Domain.Books;
using Bookstore.Domain.ReferenceData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services
{
    public interface IReferenceDataService
    {
        IEnumerable<ReferenceDataItem> GetReferenceData(ReferenceDataType? referenceDataType = null);

        Task SaveAsync(ReferenceDataItem referenceDataItem, string createdBy);

        ReferenceDataItem GetReferenceDataItem(int id);
    }

    public class ReferenceDataService : IReferenceDataService
    {
        private readonly IGenericRepository<ReferenceDataItem> referenceDataRepository;

        public ReferenceDataService(IGenericRepository<ReferenceDataItem> referenceDataRepository)
        {
            this.referenceDataRepository = referenceDataRepository;
        }

        public IEnumerable<ReferenceDataItem> GetReferenceData(ReferenceDataType? referenceDataType = null)
        {
            return referenceDataType.HasValue ?
             referenceDataRepository.Get(x => x.DataType == referenceDataType) :
             referenceDataRepository.Get();
        }

        public ReferenceDataItem GetReferenceDataItem(int id)
        {
            return referenceDataRepository.Get(id);
        }

        public async Task SaveAsync(ReferenceDataItem referenceDataItem, string createdBy)
        {
            if (referenceDataItem.IsNewEntity()) referenceDataItem.CreatedBy = createdBy;

            referenceDataItem.UpdatedOn = DateTime.UtcNow;

            referenceDataRepository.AddOrUpdate(referenceDataItem);

            await referenceDataRepository.SaveAsync();
        }
    }
}