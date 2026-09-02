using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bookstore.Domain.ReferenceData
{
    [Table("referencedata", Schema = "bobsusedbookstore_dbo")]
    public class ReferenceDataItem : Entity
    {
        // An empty constructor is required by EF Core
        private ReferenceDataItem() { }

        public ReferenceDataItem(ReferenceDataType referenceDataType, string text)
        {
            DataType = referenceDataType;
            Text = text;
        }

        [Column("datatype")]
        public ReferenceDataType DataType { get; set; }

        [Column("text")]
        public string Text { get; set; }
    }
}
