using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace yakov.ExchangeRates.Server.Domain.Entities.RemoteAPIs.Coin
{
    public class CurrencyCoin
    {
        [JsonPropertyName("asset_id")]
        public string ShortName { get; set; }
    }
}
