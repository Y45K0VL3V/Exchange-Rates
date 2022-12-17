namespace yakov.ExchangeRates.Server.Domain.Interfaces
{
    public interface ISavedRatesLoaderService
    {
        public Task LoadAll();
        public Task SaveAll();
    }
}
