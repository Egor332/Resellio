namespace ResellioBackend.TicketPurchaseSystem.Services.Abstractions
{
    public interface ISellerRegistrationStateService
    {
        public Task<string> CreateAndStoreStateAsync(int userId);

        public Task<int?> ValidateStateAsync(string  state);

        public Task RemoveStateAsync(string state);
    }
}
