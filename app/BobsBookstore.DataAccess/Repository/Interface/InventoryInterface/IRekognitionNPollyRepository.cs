using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp.Formats;
using Amazon.Polly;

namespace BobsBookstore.DataAccess.Repository.Interface.Implementations
{
    public interface IRekognitionNPollyRepository
    {
        public Task<bool> IsBook(string bucket, string key);

        public  Task<bool> IsImageSafe(string bucket, string key);

        public bool IsContentViolation(string input);

        public  Task<Stream> ResizeImage(IFormFile file, string fileExt);

        public IImageEncoder selectEncoder(string extension);

        public string GenerateAudioSummary(string BookName, string Summary, string targetLanguageCode, VoiceId voice);

        public  Task<string> UploadtoS3(IFormFile file, long BookId, string Condition);
    }
}
