using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace yakov.ExchangeRates.Server.Domain.Entities
{
    public enum CurrencyType
    {
        Fiat,
        Crypto
    }

    public struct Currency
    {
        public CurrencyType Type { get; set; }
        public string ShortName { get; set; }
    }
}
