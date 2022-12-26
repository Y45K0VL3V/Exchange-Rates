using yakov.ExchangeRates.Server.Domain.Entities;
using yakov.ExchangeRates.Server.Domain.Entities.RemoteAPIs.Coin;

namespace yakov.ExchangeRates.Server.Application.Mappers
{
    public static class CoinExtension
    {
        public static Currency ToCurrency(this CurrencyCoin currencyCoin)
        {
            return new()
            {
                ShortName = currencyCoin.ShortName,
                Type = CurrencyType.Crypto,
            };
        }

        public static Rate ToRate(this RateCoin rateCoin, Currency currency, int scaleAmount)
        {
            return new()
            {
                Currency = currency,
                Date = DateOnly.FromDateTime(rateCoin.Date),
                Value = (rateCoin.RateHigh + rateCoin.RateLow) / 2,
                Amount = scaleAmount,
            };
        }
    }
}
