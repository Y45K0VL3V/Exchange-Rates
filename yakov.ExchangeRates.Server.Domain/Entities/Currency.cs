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

    public class Currency
    {
        public CurrencyType Type { get; set; }
        public string ShortName { get; set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as Currency);
        }

        public bool Equals(Currency other)
        {
            return other != null &&
                   Type == other.Type &&
                   ShortName == other.ShortName;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Type, ShortName);
        }
    }
}
