using System;
using System.Collections.Generic;
using System.Text;
using BOBS_Backend.Database;
using System.Threading.Tasks;
using BOBS_Backend.Models.Book;
using System.Security.Claims;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Autofac.Extras.Moq;
using Bobs_Backend.Test.Repository;
using BOBS_Backend;
using BOBS_Backend.Repository.Implementations;
using Amazon.Runtime.Internal.Util;
using BOBS_Backend.Repository.SearchImplementations;
using Microsoft.Extensions.Logging;
using Xunit;
using Bobs_Backend;
using Moq;
using Microsoft.EntityFrameworkCore.Storage;
using BOBS_Backend.Repository.Implementations.InventoryImplementation;
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
using System.Threading;
namespace Bobs_Backend.Test.Repository.Implementation.InventoryImplemetationTests

{
    public class RekognitionNPollyTests
    {

        private readonly  RekognitionNPollyRepository _sut;
        private readonly Mock<Microsoft.Extensions.Logging.ILogger<RekognitionNPollyRepository>> _mockLogger = new Mock<Microsoft.Extensions.Logging.ILogger<RekognitionNPollyRepository>>();
        private readonly Mock<IAmazonS3> _mockAmazonS3 = new Mock<IAmazonS3>();
        private readonly Mock<IAmazonRekognition> _mockAmazonRekognition = new Mock<IAmazonRekognition>();
        private readonly Mock<IAmazonPolly> _mockAmazonPolly = new Mock<IAmazonPolly>();
        private readonly Mock<IHostingEnvironment> _mockHostingEnvironment = new Mock<IHostingEnvironment>();

        [Fact]
        public async Task UploadToS3_ShouldReturnNull_WhenNotRightFileType()
        {
            //Arrange
            var fileMock = new Mock<IFormFile>();
            //Setup mock file using a memory stream
            var content = "Hello World from a Fake File";
            var fileName = "test.pdf";
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write(content);
            writer.Flush();
            ms.Position = 0;
            fileMock.Setup(_ => _.OpenReadStream()).Returns(ms);
            fileMock.Setup(_ => _.FileName).Returns(fileName);
            fileMock.Setup(_ => _.Length).Returns(ms.Length);
            var file = fileMock.Object;

            //Act
            MockDatabaseRepo connect = new MockDatabaseRepo();
            var context = connect.CreateInMemoryContext();
            RekognitionNPollyRepository _sut = new RekognitionNPollyRepository(context,_mockHostingEnvironment.Object ,_mockAmazonS3.Object,_mockAmazonRekognition.Object,_mockAmazonPolly.Object,_mockLogger.Object);
            string Condition = "Old";
            long BookId = 12;
            var url = await _sut.UploadtoS3(file ,BookId, Condition);
            Assert.Equal("InvalidFileType", url);
            context.Dispose();
        }

        [Fact]
       public async Task IsImageSafe_ShouldReturnTrueIfSafe_WhenBucketDetailsAreValidandImageNotProfane()
        {
            MockDatabaseRepo connect = new MockDatabaseRepo();
            var context = connect.CreateInMemoryContext();
            RekognitionNPollyRepository _sut = new RekognitionNPollyRepository(context, _mockHostingEnvironment.Object, _mockAmazonS3.Object, _mockAmazonRekognition.Object, _mockAmazonPolly.Object, _mockLogger.Object);
            var result = await _sut.IsImageSafe("bookcoverpictures", "Reboot10New.jpg");
            Assert.Equal(true, result);
        }
    }
}
