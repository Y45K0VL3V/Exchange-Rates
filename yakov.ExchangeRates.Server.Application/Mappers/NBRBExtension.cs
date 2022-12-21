using yakov.ExchangeRates.Server.Domain.Entities;
using yakov.ExchangeRates.Server.Domain.Entities.RemoteAPIs;

namespace yakov.ExchangeRates.Server.Application.Mappers
{
    public static class NBRBExtension
    {
        public static Currency ToCurrency(this CurrencyNBRB currencyNBRB)
        {
            return new()
            {
                ShortName = currencyNBRB.Cur_Abbreviation,
                Type = CurrencyType.Fiat,
            };
        }

        public static Rate ToRate(this ShortRateNBRB shortRateNBRB, Currency currency, int scaleAmount)
        {
            return new()
            {
                Currency = currency,
                Date = DateOnly.FromDateTime(shortRateNBRB.Date),
                Value = shortRateNBRB.Cur_OfficialRate!.Value,
                Amount = scaleAmount,
            };
        }
    }
}
