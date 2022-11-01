using Bookstore.Data.Repository.Interface;
using Bookstore.Domain.ReferenceData;
using System.Collections.Generic;
using System.Linq;

namespace Services
{
    public interface IReferenceDataService
    {
        IEnumerable<ReferenceDataItem> GetReferenceData(ReferenceDataType referenceDataType);

        IEnumerable<ReferenceDataItem> GetAllReferenceData();

        void Add(ReferenceDataType dataType, string text, string createdBy);
    }

    public class ReferenceDataService : IReferenceDataService
    {
        private readonly IGenericRepository<ReferenceDataItem> referenceDataRepository;

        public ReferenceDataService(IGenericRepository<ReferenceDataItem> referenceDataRepository)
        {
            this.referenceDataRepository = referenceDataRepository;
        }

        public void Add(ReferenceDataType dataType, string text, string createdBy)
        {
            var formattedText = text.Trim();

            if(referenceDataRepository.Get2(x => x.DataType == dataType && x.Text == text).SingleOrDefault() != null) return;

            referenceDataRepository.Add(new ReferenceDataItem() { DataType = dataType, Text = text, CreatedBy = createdBy });

            referenceDataRepository.Save();
        }

        public IEnumerable<ReferenceDataItem> GetAllReferenceData()
        {
            return referenceDataRepository.Get2();
        }

        public IEnumerable<ReferenceDataItem> GetReferenceData(ReferenceDataType referenceDataType)
        {
            return referenceDataRepository.Get2(x => x.DataType == referenceDataType);
        }        
    }
}
