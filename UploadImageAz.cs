using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;


namespace CTAI.lab3UploadImage
{
    public class UploadImageAz
    {
        private readonly ILogger<UploadImageAz> _logger;
        private readonly BlobServiceClient _blobServiceClient;

        public UploadImageAz(ILogger<UploadImageAz> logger, BlobServiceClient BlobServiceClient)
        {
            _logger = logger;
            _blobServiceClient = BlobServiceClient;
        }

        [Function("UploadImage")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "uploadimage")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request for uploading an image.");

            try
            {
                // Check if any files are uploaded
                if (req.Form.Files.Count == 0)
                {
                    return new BadRequestObjectResult("No files were uploaded.");
                }

                var file = req.Form.Files[0];

                // Create a unique filename
                string fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

                // Get a reference to the container
                var containerClient = _blobServiceClient.GetBlobContainerClient("images");

                // Ensure the container exists
                await containerClient.CreateIfNotExistsAsync();

                // Get a reference to the blob
                var blobClient = containerClient.GetBlobClient(fileName);

                // Upload the file to blob storage
                await using var stream = file.OpenReadStream();
                await blobClient.UploadAsync(stream, new Azure.Storage.Blobs.Models.BlobUploadOptions
                {
                    HttpHeaders = new Azure.Storage.Blobs.Models.BlobHttpHeaders
                    {
                        ContentType = file.ContentType
                    }
                });

                return new CreatedResult(blobClient.Uri, $"File uploaded successfully: {blobClient.Uri}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while uploading the file: {ex.Message}");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
