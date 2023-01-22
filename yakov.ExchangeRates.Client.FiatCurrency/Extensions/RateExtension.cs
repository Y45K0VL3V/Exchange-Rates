using LiveChartsCore.Defaults;
using yakov.ExchangeRates.Client.Business;

namespace yakov.ExchangeRates.Client.FiatCurrency.Extensions
{
    public static class RateExtension
    {
        public static DateTimePoint ToDateTimePoint(this Rate rate)
        {
            return new DateTimePoint(rate.Date.ToDateTime(new()), (double?)rate.Value);
        }
    }
}
