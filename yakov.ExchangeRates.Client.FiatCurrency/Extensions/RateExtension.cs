using LiveChartsCore.Defaults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
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
