namespace ResellioBackend.EventManagementSystem.Services.Abstractions
{
    public interface IQRCodeService
    {
        public Task<byte[]> GenerateQRCodeAsync(Guid ticketId);
    }
}
