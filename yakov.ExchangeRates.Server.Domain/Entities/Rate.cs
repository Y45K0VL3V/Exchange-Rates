using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace yakov.ExchangeRates.Server.Domain.Entities
{
    public enum CyrrencyType
    {
        Fiat,
        Crypto
    }

    public class Rate
    {
        public string CurrencySymbol { get; set; }
        public CyrrencyType CyrrencyType { get; set; }
        public DateOnly Date { get; set; }
        public decimal Value { get; set; }
        public decimal Amount { get; set; }
    }
}
