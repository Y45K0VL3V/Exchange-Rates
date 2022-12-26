using yakov.ExchangeRates.Server.Domain.Entities;
using yakov.ExchangeRates.Server.Domain.Interfaces;

namespace yakov.ExchangeRates.Server.Infrastructure.RemoteAPIServices
{
    public class RatesAPIServiceBuilder : IAPIServiceBuilder
    {
        public IRatesAPIService BuildAPIService(CurrencyType currencyType)
        {
            switch (currencyType)
            {
                case CurrencyType.Fiat:
                    return new NBRBService();

                case CurrencyType.Crypto:
                    return new CoinService();
                
                default: 
                    throw new ArgumentException();
            }
        }
    }
}
