using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using ResellioBackend.EventManagementSystem.ObjectStorages.Abstractions;

namespace ResellioBackend.EventManagementSystem.ObjectStorages.Implementations
{
    public class AzureImageStorage : IImageStorage
    {
        private readonly string _connectionString;
        private readonly string _containerName;

        public AzureImageStorage(IConfiguration configuration)
        {
            _connectionString = configuration["ObjectStorage:ConnectionString"];
            _containerName = configuration["ObjectStorage:ImageContainerName"];
            if (string.IsNullOrEmpty(_connectionString) || string.IsNullOrWhiteSpace(_containerName)) 
            {
                throw new ArgumentException("Environment variables was not provided");
            }
        }

        public async Task<string> UploadImageAsync(IFormFile image)
        {
            var blobContainerClient = new BlobContainerClient(_connectionString, _containerName);
            var httpHeaders = new BlobHttpHeaders
            {
                ContentType = "image/jpeg"
            };
            var uniqueFileName = $"{Path.GetFileNameWithoutExtension(image.FileName)}_{Guid.NewGuid()}{Path.GetExtension(image.FileName)}";
            var blobClient = blobContainerClient.GetBlobClient(uniqueFileName);
            await using var memoryStream = new MemoryStream();
            await image.CopyToAsync(memoryStream);
            memoryStream.Position = 0;
            await blobClient.UploadAsync(memoryStream, new BlobUploadOptions
            {
                HttpHeaders = httpHeaders
            });
            var uri = blobClient.Uri.AbsoluteUri;

            return uri;
        }
    }
}
