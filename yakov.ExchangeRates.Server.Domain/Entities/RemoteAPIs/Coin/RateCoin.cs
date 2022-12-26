using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace yakov.ExchangeRates.Server.Domain.Entities.RemoteAPIs.Coin
{
    public class RateCoin
    {
        [JsonPropertyName("time_period_start")]
        public DateTime Date { get; set; }
        [JsonPropertyName("rate_high")]
        public decimal RateHigh { get; set; }
        [JsonPropertyName("rate_low")]
        public decimal RateLow { get; set; }
    }
}
