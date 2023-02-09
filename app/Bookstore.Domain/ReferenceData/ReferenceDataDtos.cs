namespace Bookstore.Domain.ReferenceData
{
    public record CreateReferenceDataItemDto(ReferenceDataType ReferenceDataType, string Text);

    public record UpdateReferenceDataItemDto(int Id, ReferenceDataType ReferenceDataType, string Text);
}
