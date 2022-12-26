using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace yakov.ExchangeRates.Server.Domain.Entities.RemoteAPIs.NBRB
{
    public class RateNBRB
    {
        [JsonPropertyName("Cur_ID")]
        public int ID { get; set; }
        public DateTime Date { get; set; }
        [JsonPropertyName("Cur_OfficialRate")]
        public decimal? CurrRate { get; set; }
        [JsonPropertyName("Cur_Scale")]
        public int CurrScale { get; set; }
    }
}
