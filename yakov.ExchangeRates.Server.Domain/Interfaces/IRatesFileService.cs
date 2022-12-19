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
        public Task<Dictionary<Currency, List<Rate>>> GetSavedRates();
        public Task<List<Rate>?> GetSavedRatesByCurrency(Currency currency);
        public Task WriteRatesByCurrency(List<Rate> rates, Currency currency);
    }
}
