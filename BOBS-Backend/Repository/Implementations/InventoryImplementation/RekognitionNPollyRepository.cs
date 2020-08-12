using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon;
using Amazon.Rekognition;
using Amazon.Rekognition.Model;
using Amazon.S3;
using Amazon.S3.Transfer;
using BOBS_Backend.Database;
using Microsoft.AspNetCore.Http;
using System.IO;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Gif;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using Amazon.Polly;
using Microsoft.AspNetCore.Hosting;
using System.Linq;
using BOBS_Backend.Models.Book;
using Microsoft.Extensions.Logging;

namespace BOBS_Backend.Repository.Implementations.InventoryImplementation
{
    public class RekognitionNPollyRepository : IRekognitionNPollyRepository
    {
        IAmazonS3 _s3Client { get; set; }
        IAmazonRekognition _rekognitionClient { get; set; }

        IAmazonPolly _pollyClient { get; set; }

        private readonly ILogger<RekognitionNPollyRepository> _logger;
        public DatabaseContext _context;
        private IHostingEnvironment _env;

        public RekognitionNPollyRepository(DatabaseContext context, IHostingEnvironment env , IAmazonS3 s3Client , IAmazonRekognition rekognitionClient, IAmazonPolly pollyClient , ILogger<RekognitionNPollyRepository> logger)
        {
            _context = context;
            _env = env;
            _s3Client = s3Client;
            _rekognitionClient = rekognitionClient;
            _pollyClient = pollyClient;
            _logger = logger;
        }

        /*
        *  function to  process  and push user uploaded book cover pictures to S3 bucket 
        */
        public async Task<string> UploadtoS3(IFormFile file , long BookId , string Condition)
        {
            _logger.LogInformation("Uploading Picture to S3 Bucket");

            var split = file.FileName.Split(".");
            string filename = split[0] + BookId.ToString() + Condition + "."+split[1];
            var dir = _env.ContentRootPath;
            string url = "";

            HashSet<string> _validImageExtensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "jpg", "jpeg", "png", "gif"
        };

            var fileExt = Path.GetExtension(file.FileName).TrimStart('.');

            var resizeStream = await ResizeImage(file, fileExt);

            if (_validImageExtensions.Contains(fileExt))
            {
                using (var fileStream = new FileStream(Path.Combine(dir, filename), FileMode.Create, FileAccess.Write))
                {
                    resizeStream.CopyTo(fileStream);
                }

                var fileTransferUtility = new TransferUtility(_s3Client);

                await fileTransferUtility.UploadAsync(Path.Combine(dir, filename), Constants.photosBucketName);


                bool check = await IsImageSafe(Constants.photosBucketName, filename);

                if (check)
                {
                    if (await IsBook(Constants.photosBucketName, filename))
                    {
                        url = String.Concat(Constants.CoverPicturesCloudFrontLink, filename);
                        return url;

                    }
                    else
                    {
                        return $"NotABook";

                    }
                }

                else
                {
                    return $"PolicyViolation";
                }

            }
            return $"InvalidFileType";
        }


        /*
       *  function to  check if uplaoded picture is of a book 
       */

        public async Task<bool> IsBook(string bucket, string key)
        {
            _logger.LogInformation("Checking if uploaded picture is of a Book");
            HashSet<string> validlabels = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "Book", "Paper", "Magazine", "Newspaper" ,"Storybook" ,"Textbook" , "Novel" ,"Flyer" ,"Text" ,"Poster" , "Brochure" ,"Advertisment"
        };

            DetectLabelsRequest detectlabelsRequest = new DetectLabelsRequest()
            {
                Image = new Amazon.Rekognition.Model.Image
                {
                    S3Object = new Amazon.Rekognition.Model.S3Object
                    {
                        Name = key,
                        Bucket = bucket
                    },
                },
                MaxLabels = 8,
                MinConfidence = 75F
            };

            int c = 0;
            DetectLabelsResponse detectLabelsResponse = await _rekognitionClient.DetectLabelsAsync(detectlabelsRequest);
            foreach (Label label in detectLabelsResponse.Labels)
                if (validlabels.Contains(label.Name) && label.Confidence >= 70)
                {

                    c++;

                }

            if (c >= 1)
            {

                return true;
            }

            else
            {
                return false;
            }

        }

        /*
       *  function to check uloaded pictures for profanity
       */
        public async Task<bool> IsImageSafe(string bucket, string key)
        {
            _logger.LogInformation("Content Moderation : Checks for profanity");
        //    var RekognitionClient = new AmazonRekognitionClient("AKIA6DYNIKQLFG4HU2LU", "4RM8WQL3tH4+c7RgIQ/LPBHqt6ESwleokqsDx1Gf", bucketRegion);
            var response = await _rekognitionClient.DetectModerationLabelsAsync(new DetectModerationLabelsRequest
            {
                Image = new Amazon.Rekognition.Model.Image
                {
                    S3Object = new Amazon.Rekognition.Model.S3Object
                    {
                        Bucket = bucket,
                        Name = key
                    }
                }
            });

            if (response.ModerationLabels.Count > 0)
            {
                return false;
            }

            else
            {
                return true;
            }
        }

        public bool IsContentViolation(string input)
        {

            if (input == "NotABook" || input == "PolicyViolation" || input == "InvalidFileType")
            {
                return true;
            }

            else
            {
                return false;
            }
        }

        /*
       *  function to  resize uploaded book cover pictures bwfore being pushed to S3 bucket 
       */
        public async Task<Stream> ResizeImage(IFormFile file, string fileExt)
        {
            _logger.LogInformation("Resizing Image");
            // create new memory stream.
            Stream result = new MemoryStream();
            // create new image variable
           var img = SixLabors.ImageSharp.Image.Load(file.OpenReadStream());
               // change size of image
            img.Mutate(x => x.Resize(Constants.resize_width, Constants.resize_height));
            //get the extension encoder
            IImageEncoder encoder = selectEncoder(fileExt);
            img.Save(result, encoder);
            result.Position = 0;

            return result;


        }

        /*
       *  function to  encode the uplaoded images   
       */
        public IImageEncoder selectEncoder(string extension)
        {
            _logger.LogInformation("Encoding Picture");
            IImageEncoder encoder = null;
            // get the encoder based on file extension
            switch (extension)
            {
                case "png":
                case "PNG":
                    encoder = new PngEncoder();
                    break;
                case "jpeg":
                case "JPEG":
                    encoder = new JpegEncoder();
                    break;
                case "jpg":
                case "JPG":
                    encoder = new JpegEncoder();
                    break;
                case "gif":
                case "GIF":
                    encoder = new GifEncoder();
                    break;
                default:
                    break;
            }
            return encoder;
        }



        /*
       *  function to convert text summary to speech 
       */
        public string GenerateAudioSummary(string BookName, string Summary, string targetLanguageCode, VoiceId voice)
        {
            _logger.LogInformation("Converting text to speech");
            using (_pollyClient)
            {
                var request = new Amazon.Polly.Model.SynthesizeSpeechRequest();
                request.LanguageCode = targetLanguageCode;
                request.Text = Summary;
                request.OutputFormat = OutputFormat.Mp3;
                request.VoiceId = voice;
                var response = _pollyClient.SynthesizeSpeechAsync(request).GetAwaiter().GetResult();

                string outputFileName = $".\\-{targetLanguageCode}.mp3";
                FileStream output = File.Open(outputFileName, FileMode.Create);
                response.AudioStream.CopyTo(output);
                output.Close();
                var fileTransferUtility = new TransferUtility(_s3Client);
                fileTransferUtility.UploadAsync(outputFileName,Constants.audioBucketName);
                var url = String.Concat(Constants.AudioFilesCloudFrontLink, BookName);
                return url;
            }
        }





    }
}
