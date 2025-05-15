namespace ResellioBackend.TicketPurchaseSystem.Services.Abstractions
{
    public interface ISellerRegistrationService
    {
        public Task<string> StartRegistrationAsync(int userId, string hostInfo);

        public Task<bool> CompleteRegistrationAsync(string code, string state);
    }
}
