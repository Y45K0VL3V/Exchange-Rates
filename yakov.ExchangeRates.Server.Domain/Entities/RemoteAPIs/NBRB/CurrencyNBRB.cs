using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace yakov.ExchangeRates.Server.Domain.Entities.RemoteAPIs.NBRB
{
    public class CurrencyNBRB
    {
        [JsonPropertyName("Cur_ID")]
        public int CurrID { get; set; }
        [JsonPropertyName("Cur_Abbreviation")]
        public string CurrAbbreviation { get; set; }
        [JsonPropertyName("Cur_Scale")]
        public int CurrScale { get; set; }
    }
}
