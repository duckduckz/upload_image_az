using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace CTAI.lab3UploadImage
{
    public class NewImageDetected
    {
        private readonly ILogger<NewImageDetected> _logger;

        public NewImageDetected(ILogger<NewImageDetected> logger)
        {
            _logger = logger;
        }

        [Function(nameof(NewImageDetected))]
        public async Task Run([BlobTrigger("images/{name}", Connection = "StorageAccount")] Stream stream, string name)
        {
            _logger.LogInformation($"C# Blob trigger function started processing blob\n Name: {name}");

            try
            {
                // Read the content of the uploaded blob
                using var blobStreamReader = new StreamReader(stream);
                var content = await blobStreamReader.ReadToEndAsync();

                _logger.LogInformation($"C# Blob trigger function processed blob\n Name: {name} \n Data: {content}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error processing blob {name}: {ex.Message}");
                throw;
            }
        }
    }
}