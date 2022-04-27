using Amazon;

public abstract class Constants
{
    public const int TotalResults = 8;
    public const int BooksPerPage = 15;
    public const int ResizeWidth = 200;
    public const int ResizeHeight = 200;

    public const string BookDetailsStatusList = "List";
    public const string TextToSpeechLanguageCode = "fr-CA";
    public const string OrderStatusDelivered = "Delivered";
    public const string OrderStatusJustPlaced = "Just Placed";
    public const string OrderStatusEnRoute = "En Route";
    public const string OrderStatusPending = "Pending";
    public const string ViewStyleDefault = "Tabular";
    public const string SortByNamePhrase = "Name";
    public const string SortByPricePhrase = "ItemPrice";
    public const string SortByQuantityPhrase = "Quantity";
    public const string SortByAuthorPhrase = "Author";
    public const string LambdaExpressionType = "BobsBookstore.Models.Books.Price";
    public const string LambdaExpressionPhrase = "OrderDetailPrice";
    public const string ResaleStatusPending = "Pending Approval";
    public const string ResaleStatusApproved = "Approved/Awaiting Shipment";
    public const string ResaleStatusRejected = "Rejected";

    public RegionEndpoint BucketRegion = RegionEndpoint.USWest2;
}