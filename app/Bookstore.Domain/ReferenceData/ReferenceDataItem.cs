namespace Bookstore.Domain.ReferenceData
{
    public class ReferenceDataItem : Entity<int>
    {
        public ReferenceDataType DataType { get; set; }

        public string Text { get; set; }
    }
}
