using Amazon;

namespace BookstoreBackend
{
    public class Constants
    {
        public const int TotalResults = 8;
        public const int BooksPerPage = 15;
        public const int ResizeWidth = 200;
        public const int ResizeHeight = 200;

        public const string ErrorStatusYes = "Yes";
        public const string GenreExistsStatus = "Given genre already exists in the database";
        public const string TypeExistsStatus = "Given format already exists in the database";
        public const string ConditionExistsStatus = "Given condition already exists in the database";
        public const string PublisherExistsStatus = "Given publisher already exists in the database";

        public const string AddPublisherMessage =
            "Please enter the details of the publisher you wish to add to the database";

        public const string AddGenreMessage = "Please enter the details of the Genre you wish to add to the database";

        public const string AddConditionsMessage =
            "Please enter the details of the condition you wish to add to the database";

        public const string AddTypeMessage = "Please enter the details of the format you wish to add to the database";

        public const string BookDetailsStatusDetails = "details";
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
        public const string LambdaExpressionType = "shared_code.Models.Book.Price";
        public const string LambdaExpressionPhrase = "OrderDetailPrice";

        public const string CombinationErrorStatus =
            "Sorry, we don't currently have any relevant results for the given combination";

        public const string BoBsEmailAddress = "";
        public const string ResaleStatusPending = "Pending Approval";
        public const string ResaleStatusApproved = "Approved/Awaiting Shipment from Customer";
        public const string ResaleStatusRejected = "Rejected";
        public const string ResaleStatusReceived = "Shipment Receipt Confirmed";
        public const string ResaleStatusPaymentCompleted = "Payment Completed";

        public RegionEndpoint BucketRegion = RegionEndpoint.USWest2;
    }
}
