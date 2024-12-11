using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace CTAI.lab3UploadImage
{
    public class UploadImageAz
    {
        private readonly ILogger<UploadImageAz> _logger;

        public UploadImageAz(ILogger<UploadImageAz> logger)
        {
            _logger = logger;
        }

        [Function("UploadImageAz")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            return new OkObjectResult("Welcome to Azure Functions!");
        }
    }
}
