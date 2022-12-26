using yakov.ExchangeRates.Server.Domain.Entities;

namespace yakov.ExchangeRates.Server.Domain.Interfaces
{
    public interface ISavedRatesLoaderService
    {
        public Task LoadAll();
        public Task SaveAll();
        public Task SaveByCurrency(Currency currency);
    }
}
