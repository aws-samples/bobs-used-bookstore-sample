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
        public string ErrorStatusYes = "Yes";
        public string GenreExistsStatus = "Given Genre already exists in the database";
        public string TypeExistsStatus = "Given Format already exists in the database";
        public string ConditionExistsStatus = "Given Condition already exists in the database";
        public string PublisherExistsStatus = "Given Publisher already exists in the database";
        public string BookDetailsStatusDetails = "details";
        public string BookDetailsStatusList = "List";
        public int BooksPerPage = 15;
        public string TextToSpeechLanguageCode = "fr-CA";
        public string OrderStatusDelivered = "Delivered";
        public string OrderStatusJustPlaced = "Just Placed";
        public string OrderStatusEnRoute = "En Route";
        public string OrderStatusPending = "Pending";
        public string ViewStyleDefault = "Tabular";
        public string SortByNamePhrase = "Name";
        public string SortByPricePhrase = "ItemPrice";
        public string SortByQuantityPhrase = "Quantity";
        public string SortByAuthorPhrase = "Author";
        public string LambdaExpressionType = "BOBS_Backend.Models.Book.Price";
        public string LambdaExpressionPhrase = "price";
        public string photosBucketName = "bookcoverpictures";
        public string audioBucketName = "audiosummary";
        public RegionEndpoint bucketRegion = RegionEndpoint.USEast1;
        public string CoverPicturesCloudFrontLink = "https://dtdt6j0vhq1rq.cloudfront.net/";
        public string AudioFilesCloudFrontLink = "https://d3iukz826t8vlr.cloudfront.net/";
        public int resize_width = 200;
        public int resize_height = 200;    
    }
