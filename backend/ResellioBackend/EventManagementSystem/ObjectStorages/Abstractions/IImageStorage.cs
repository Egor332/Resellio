namespace ResellioBackend.EventManagementSystem.ObjectStorages.Abstractions
{
    public interface IImageStorage
    {
        public Task<string> UploadImageAsync(IFormFile image);
    }
}
