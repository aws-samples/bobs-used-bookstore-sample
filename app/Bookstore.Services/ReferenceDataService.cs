using Bookstore.Data.Repository.Interface;
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
        IEnumerable<ReferenceDataItem> GetReferenceData(ReferenceDataType referenceDataType);

        IEnumerable<ReferenceDataItem> GetAllReferenceData();

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

        public IEnumerable<ReferenceDataItem> GetAllReferenceData()
        {
            return referenceDataRepository.Get2();
        }

        public IEnumerable<ReferenceDataItem> GetReferenceData(ReferenceDataType referenceDataType)
        {
            return referenceDataRepository.Get2(x => x.DataType == referenceDataType);
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