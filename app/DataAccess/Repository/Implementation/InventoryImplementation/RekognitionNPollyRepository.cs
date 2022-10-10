using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Amazon.Polly;
using Amazon.Polly.Model;
using Amazon.Rekognition;
using Amazon.Rekognition.Model;
using Amazon.S3;
using Amazon.S3.Transfer;
using DataAccess.Data;
using DataAccess.Repository.Interface.Implementations;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Gif;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Processing;
using S3Object = Amazon.Rekognition.Model.S3Object;

namespace DataAccess.Repository.Implementation.InventoryImplementation
{
    public class RekognitionNPollyRepository : IRekognitionNPollyRepository
    {
        private readonly ILogger<RekognitionNPollyRepository> _logger;
        private readonly IConfiguration _configuration;
        public ApplicationDbContext _context;
        private IWebHostEnvironment _env;

        public RekognitionNPollyRepository(IConfiguration configuration, ApplicationDbContext context,
            IWebHostEnvironment env, IAmazonS3 s3Client, IAmazonRekognition rekognitionClient, IAmazonPolly pollyClient,
            ILogger<RekognitionNPollyRepository> logger)
        {
            _context = context;
            _env = env;
            _s3Client = s3Client;
            _rekognitionClient = rekognitionClient;
            _pollyClient = pollyClient;
            _logger = logger;
            _configuration = configuration;
        }

        private IAmazonS3 _s3Client { get; }
        private IAmazonRekognition _rekognitionClient { get; }
        private IAmazonPolly _pollyClient { get; }

        /*
        *  function to  process  and push user uploaded book cover pictures to S3 bucket 
        */
        public async Task<string> UploadtoS3Async(IFormFile file, long BookId, string Condition)
        {
            _logger.LogInformation("Uploading Picture to S3 Bucket");

            var split = file.FileName.Split(".");
            var filename = split[0] + BookId + Condition + "." + split[1];
            //var dir = _env.ContentRootPath;
            var dir = Path.GetTempPath();
            var url = "";
            var bucketName = _configuration["AWS:BucketName"];

            var _validImageExtensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "jpg", "jpeg", "png", "gif"
            };

            var fileExt = Path.GetExtension(file.FileName).TrimStart('.');

            if (_validImageExtensions.Contains(fileExt))
            {
                var resizeStream = await ResizeImage(file, fileExt);

                using (var fileStream = new FileStream(Path.Combine(dir, filename), FileMode.Create, FileAccess.Write))
                {
                    resizeStream.CopyTo(fileStream);
                    File.SetAttributes(Path.Combine(dir, filename), FileAttributes.Normal);
                }

                var fileTransferUtility = new TransferUtility(_s3Client);
                var combine = Path.Combine(dir, filename);
                fileTransferUtility.Upload(combine, bucketName);


                var check = await IsImageSafe(bucketName, filename);
                if (check)
                {
                    url = string.Concat(_configuration["AWS:CloudFrontDomain"], "/", filename);
                    File.SetAttributes(Path.Combine(dir, filename), FileAttributes.Normal);
                    File.Delete(Path.Combine(dir, filename));

                    return url;
                }

                {
                    return "PolicyViolation";
                }
            }

            return "InvalidFileType";
        }


        /*
       *  function to  check if uplaoded picture is of a book 
       */

        public async Task<bool> IsBook(string bucket, string key)
        {
            _logger.LogInformation("Checking if uploaded picture is of a Book");
            var validlabels = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "Book", "Paper", "Magazine", "Newspaper", "Storybook", "Textbook", "Novel", "Flyer", "Text", "Poster",
                "Brochure", "Advertisment"
            };

            var detectlabelsRequest = new DetectLabelsRequest
            {
                Image = new Image
                {
                    S3Object = new S3Object
                    {
                        Name = key,
                        Bucket = bucket
                    }
                },
                MaxLabels = 8,
                MinConfidence = 75F
            };

            var c = 0;
            var detectLabelsResponse = await _rekognitionClient.DetectLabelsAsync(detectlabelsRequest);
            foreach (var label in detectLabelsResponse.Labels)
                if (validlabels.Contains(label.Name) && label.Confidence >= 70)
                    c++;

            if (c >= 1)
                return true;

            return false;
        }

        /*
       *  function to check uloaded pictures for profanity
       */
        public async Task<bool> IsImageSafe(string bucket, string key)
        {
            _logger.LogInformation("Content Moderation : Checks for profanity");
            var response = await _rekognitionClient.DetectModerationLabelsAsync(new DetectModerationLabelsRequest
            {
                Image = new Image
                {
                    S3Object = new S3Object
                    {
                        Bucket = bucket,
                        Name = key
                    }
                }
            });

            if (response.ModerationLabels.Count > 0)
                return false;

            return true;
        }

        public bool IsContentViolation(string input)
        {
            if (input == "NotABook" || input == "PolicyViolation" || input == "InvalidFileType")
                return true;

            return false;
        }

        /*
       *  function to  resize uploaded book cover pictures before being pushed to S3 bucket 
       */
        public async Task<Stream> ResizeImage(IFormFile file, string fileExt)
        {
            _logger.LogInformation("Resizing Image");
            Stream result = new MemoryStream();
            var img = await SixLabors.ImageSharp.Image.LoadAsync(file.OpenReadStream());
            // change size of image
            /*img.Mutate(x => x.Resize(ConstantsData.ResizeWidth, ConstantsData.ResizeHeight));*/
            img.Mutate(x => x.Resize(0, Constants.ResizeHeight));
            var encoder = selectEncoder(fileExt);
            await img.SaveAsync(result, encoder);
            result.Position = 0;

            return result;
        }

        /*
       *  function to  encode the uploaded images   
       */
        public IImageEncoder selectEncoder(string extension)
        {
            _logger.LogInformation("Encoding Picture");
            IImageEncoder encoder = null;
            // get the encoder based on file extension
            switch (extension.ToLower())
            {
                case "png":
                    encoder = new PngEncoder();
                    break;
                case "jpeg":
                    encoder = new JpegEncoder();
                    break;
                case "jpg":
                    encoder = new JpegEncoder();
                    break;
                case "gif":
                    encoder = new GifEncoder();
                    break;
            }

            return encoder;
        }


        /*
       *  function to convert text summary to speech 
       */
        public string GenerateAudioSummary(string BookName, string Summary, string targetLanguageCode, VoiceId voice)
        {
            var bucketName = _configuration["AWS:BucketName"];
            _logger.LogInformation("Converting text to speech");
            using (_pollyClient)
            {
                var request = new SynthesizeSpeechRequest();
                request.LanguageCode = targetLanguageCode;
                request.Text = Summary;
                request.OutputFormat = OutputFormat.Mp3;
                request.VoiceId = voice;
                var response = _pollyClient.SynthesizeSpeechAsync(request).GetAwaiter().GetResult();

                var outputFileName = $".\\{BookName}-{targetLanguageCode}.mp3";

                var output = File.Open(outputFileName, FileMode.Create);
                response.AudioStream.CopyTo(output);
                output.Close();
                var fileTransferUtility = new TransferUtility(_s3Client);
                fileTransferUtility.UploadAsync(outputFileName, bucketName);
                var url = string.Concat(_configuration["AWS:CloudFrontDomain"], "/", BookName);
                return url;
            }
        }
    }
}
