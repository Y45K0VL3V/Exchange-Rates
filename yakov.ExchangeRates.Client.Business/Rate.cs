using System.Text.Json.Serialization;
using yakov.ExchangeRates.Client.Business.Converters;

namespace yakov.ExchangeRates.Client.Business
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