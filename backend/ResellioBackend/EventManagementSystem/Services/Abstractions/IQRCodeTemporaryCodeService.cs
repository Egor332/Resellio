namespace ResellioBackend.EventManagementSystem.Services.Abstractions
{
    public interface IQRCodeTemporaryCodeService
    {
        public Task<Guid?> CreateAndSaveTemporaryCodeAsync();
        public Task<bool> CheckTemporaryCodeAsync(Guid code);
    }
}
