using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using yakov.ExchangeRates.Server.Domain.Entities;

namespace yakov.ExchangeRates.Server.Domain.Interfaces
{
    public interface IRatesFileService
    {
        public Task<string> GetSavedRatesByCurrency(Currency currency);
        public Task WriteRates(IEnumerable<Rate> rates);
    }
}
