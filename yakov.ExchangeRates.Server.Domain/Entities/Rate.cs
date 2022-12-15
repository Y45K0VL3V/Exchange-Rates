using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace yakov.ExchangeRates.Server.Domain.Entities
{
    public class Rate
    {
        public Currency Currency { get; set; }
        public DateOnly Date { get; set; }
        public decimal Value { get; set; }
        public decimal Amount { get; set; }
    }
}
