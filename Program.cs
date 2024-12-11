using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Azure.Storage.Blobs;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices(services => {
        services.AddSingleton(x => new BlobServiceClient(Environment.GetEnvironmentVariable("StorageAccount")));
    })
    .Build();

host.Run();
