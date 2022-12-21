using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace yakov.ExchangeRates.Server.Domain.Entities.RemoteAPIs
{
    public class ShortRateNBRB
    {
        public int Cur_ID { get; set; }
        public DateTime Date { get; set; }
        public decimal? Cur_OfficialRate { get; set; }
        public int Cur_Scale { get; set; }
    }
}
