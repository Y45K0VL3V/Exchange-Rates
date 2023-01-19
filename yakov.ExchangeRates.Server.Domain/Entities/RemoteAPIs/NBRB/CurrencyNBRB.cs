using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace yakov.ExchangeRates.Server.Domain.Entities.RemoteAPIs.NBRB
{
    public enum PeriodicityType
    {
        Daily,
        Monthly
    }
    public class CurrencyNBRB
    {
        [JsonPropertyName("Cur_ParentID")]
        public int PrevID { get; set; }
        [JsonPropertyName("Cur_ID")]
        public int CurrID { get; set; }
        [JsonPropertyName("Cur_Abbreviation")]
        public string CurrAbbreviation { get; set; }
        [JsonPropertyName("Cur_Scale")]
        public int CurrScale { get; set; }
        [JsonPropertyName("Cur_DateStart")]
        public DateTime IdChangeDate { get; set; }
        [JsonPropertyName("Cur_DateEnd")]
        public DateTime IdEndChangeDate { get; set; }
        [JsonPropertyName("Cur_Periodicity")]
        public PeriodicityType PeriodicityType { get; set; }

    }
}
