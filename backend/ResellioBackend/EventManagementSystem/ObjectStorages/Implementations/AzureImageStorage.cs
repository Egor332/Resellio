using Azure.Storage.Blobs;
using ResellioBackend.EventManagementSystem.ObjectStorages.Abstractions;

namespace ResellioBackend.EventManagementSystem.ObjectStorages.Implementations
{
    public class AzureImageStorage : IImageStorage
    {
        private readonly string _connectionString;
        private readonly string _containerName;

        public AzureImageStorage(IConfiguration configuration)
        {
            _connectionString = configuration["ObjcetStorage:ConnectionString"];
            _containerName = configuration["ObjectStorage:ImageContainerName"];
            if (string.IsNullOrEmpty(_connectionString) || string.IsNullOrWhiteSpace(_containerName)) 
            {
                throw new ArgumentException("Environment variables was not provided");
            }
        }

        public async Task<string> UploadImageAsync(IFormFile image)
        {
            var blobContainerClient = new BlobContainerClient(_containerName, _connectionString);
            var uniqueFileName = $"{Path.GetFileNameWithoutExtension(image.FileName)}_{Guid.NewGuid()}{Path.GetExtension(image.FileName)}";
            var blobClient = blobContainerClient.GetBlobClient(uniqueFileName);
            var memoryStream = new MemoryStream();
            await image.CopyToAsync(memoryStream);
            memoryStream.Position = 0;
            await blobClient.UploadAsync(memoryStream);
            var uri = blobClient.Uri.AbsoluteUri;

            return uri;
        }
    }
}
