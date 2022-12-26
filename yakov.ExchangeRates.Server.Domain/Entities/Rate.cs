using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using yakov.ExchangeRates.Server.Domain.Converters;

namespace yakov.ExchangeRates.Server.Domain.Entities
{
    public class Rate
    {
        public Currency Currency { get; set; }

        [JsonConverter(typeof(DateOnlyJsonConverter))]
        public DateOnly Date { get; set; }
        public decimal Value { get; set; }
        public decimal Amount { get; set; }
    }
}
