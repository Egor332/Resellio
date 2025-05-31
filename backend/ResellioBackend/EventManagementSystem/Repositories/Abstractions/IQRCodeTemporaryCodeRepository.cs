namespace ResellioBackend.EventManagementSystem.Repositories.Abstractions
{
    public interface IQRCodeTemporaryCodeRepository
    {
        public Task<bool> AddTemporaryCodeAsync(Guid code);
        public Task RemoveTemporaryCodeAsync(Guid code);
        public Task<bool> IsTemporaryCodeExistAsync(Guid code);
    }
}
