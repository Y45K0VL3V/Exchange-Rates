using yakov.ExchangeRates.Server.Domain.Entities;
using yakov.ExchangeRates.Server.Domain.Entities.RemoteAPIs.NBRB;

namespace yakov.ExchangeRates.Server.Application.Mappers
{
    public static class NBRBExtension
    {
        public static Currency ToCurrency(this CurrencyNBRB currencyNBRB)
        {
            return new()
            {
                ShortName = currencyNBRB.CurrAbbreviation,
                Type = CurrencyType.Fiat,
            };
        }

        public static Rate ToRate(this RateNBRB shortRateNBRB, Currency currency, int scaleAmount)
        {
            return new()
            {
                Currency = currency,
                Date = DateOnly.FromDateTime(shortRateNBRB.Date),
                Value = shortRateNBRB.CurrRate!.Value,
                Amount = scaleAmount,
            };
        }
    }
}
