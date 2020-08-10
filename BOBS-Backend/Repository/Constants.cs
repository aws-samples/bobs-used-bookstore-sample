using Amazon;
using Amazon.Rekognition.Model.Internal.MarshallTransformations;
using BOBS_Backend.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

    public class Constants
    {
        public const int TOTAL_RESULTS = 8;
        public const string ErrorStatusYes = "Yes";
        public const string GenreExistsStatus = "Given Genre already exists in the database";
        public const string TypeExistsStatus = "Given Format already exists in the database";
        public const string ConditionExistsStatus = "Given Condition already exists in the database";
        public const string PublisherExistsStatus = "Given Publisher already exists in the database";
        public const string BookDetailsStatusDetails = "details";
        public const string BookDetailsStatusList = "List";
        public const int BooksPerPage = 15;
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
        public const string LambdaExpressionType = "BOBS_Backend.Models.Book.Price";
        public const string LambdaExpressionPhrase = "price";
        public const string photosBucketName = "bookcoverpictures";
        public const string audioBucketName = "audiosummary";
        public  RegionEndpoint bucketRegion = RegionEndpoint.USEast1;
        public const string CoverPicturesCloudFrontLink = "https://dtdt6j0vhq1rq.cloudfront.net/";
        public const string AudioFilesCloudFrontLink = "https://d3iukz826t8vlr.cloudfront.net/";
        public const int resize_width = 200;
        public const int resize_height = 200;    
    }
