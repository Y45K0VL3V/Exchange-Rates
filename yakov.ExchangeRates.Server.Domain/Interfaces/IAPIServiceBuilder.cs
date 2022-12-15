using yakov.ExchangeRates.Server.Domain.Entities;

namespace yakov.ExchangeRates.Server.Domain.Interfaces
{
    public interface IAPIServiceBuilder
    {
        public IRatesAPIService BuildAPIService(CurrencyType currencyType);
    }
}
