using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace CTAI.lab3UploadImage
{
    public class ImageUploadBlobTrigger
    {
        private readonly ILogger<ImageUploadBlobTrigger> _logger;

        public ImageUploadBlobTrigger(ILogger<ImageUploadBlobTrigger> logger)
        {
            _logger = logger;
        }

        [Function(nameof(ImageUploadBlobTrigger))]
        public async Task Run([BlobTrigger("images/{name}", Connection = "StorageAccount")] Stream stream, string name)
        {
            using var blobStreamReader = new StreamReader(stream);
            var content = await blobStreamReader.ReadToEndAsync();
            _logger.LogInformation($"C# Blob trigger function Processed blob\n Name: {name} \n Data: {content}");
        }
    }
}
