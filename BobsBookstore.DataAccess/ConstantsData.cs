using Amazon;
using Amazon.Rekognition.Model.Internal.MarshallTransformations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

    public class ConstantsData
    {
        public const int TotalResults = 8;


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
        public const string LambdaExpressionType = "BobsBookstore.Models.Books.Price";
        public const string LambdaExpressionPhrase = "OrderDetailPrice";
        public const string PhotosBucketName = "bookcoverpictures";
        public const string AudioBucketName = "audiosummary";
        public  RegionEndpoint BucketRegion = RegionEndpoint.USEast1;
        public const string CoverPicturesCloudFrontLink = "https://dtdt6j0vhq1rq.cloudfront.net/";
        public const string AudioFilesCloudFrontLink = "https://d3iukz826t8vlr.cloudfront.net/";
        public const int ResizeWidth = 200;
        public const int ResizeHeight = 200;
    }
